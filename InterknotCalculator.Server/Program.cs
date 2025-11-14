using System.Net;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using Sisk.Core.Http;

namespace InterknotCalculator.Cache;

public static class Program {
    private static string RequestFilePath { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.request");
    private static string ResultFilePath  { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.result");
    
    private static bool IsRecalculating { get; set; } = false;
    
    public static async Task Main() {
        await Resources.Current.Init();
        
        var calc = new Calculator();

        using var app = HttpServer.CreateBuilder()
            .UseConfiguration(config => {
                config.AccessLogsStream = null;     // disable access logs
                config.ErrorsLogsStream = new(Console.Error);
            })
            .UseListeningPort("http://localhost:5101")
            .Build();

        app.Router.MapGet("/", () => new HttpResponse(HttpStatusCode.OK, "Inter-Knot Calculator"));

        app.Router.MapPost("/damage", request => {
            var req = JsonSerializer.Deserialize<CalcRequest>(request.Body, SerializerContext.Default.CalcRequest);
            
            if (req is null) {
                return new HttpResponse(HttpStatusCode.BadRequest, "Bad request");
            }
            
            var discs = req.Discs.Select((d, idx) =>
                new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
                    d.StatsLevels.Skip(1).Select(p => Stat.SubStats[p.Key] with {
                        Level = p.Value
                    }))).ToArray();
            
            var calcResult = calc.Calculate(req.AgentId, 
                req.WeaponId, discs, req.Team, req.Rotation, new NotoriousDullahan {
                    StunMultiplier = req.StunBonus
                });
            
            return new HttpResponse()
                .WithStatus(HttpStatusCode.OK)
                .WithContent(JsonContent.Create(calcResult, SerializerContext.Default.CalcResult));
        });
        
        app.Router.MapGet("/recalculate", () => {
            if (IsRecalculating) {
                return new(HttpStatusCode.BadRequest, "Another request is already running");
            }
            
            IsRecalculating = true;

            if (!File.Exists(RequestFilePath)) {
                return new(HttpStatusCode.NotFound, "No request file found");
            }
            
            // open binary file at request path
            using var requestFs = new FileStream(RequestFilePath, FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(requestFs);

            var header = br.ReadBytes(4);
            if (Encoding.ASCII.GetString(header) != "IKNC") {
                return new(HttpStatusCode.BadRequest, "Invalid file header");
            }
            
            var version = br.ReadByte(); // unused

            var type = br.ReadBytes(4);
            if (Encoding.ASCII.GetString(type) != "REQ\0") {
                return new(HttpStatusCode.BadRequest, "Invalid request type");
            }
            
            var length = br.ReadInt32();
            
            requestFs.Seek(3, SeekOrigin.Current); // skip padding

            using var resultFs = new FileStream(ResultFilePath, FileMode.Create, FileAccess.Write);
            using var bw = new BinaryWriter(resultFs);
            
            bw.Write("IKNC"u8);
            bw.Write(version);
            bw.Write("RES\0"u8);
            bw.Write((uint)length);
            bw.Write("\0\0\0"u8);
            
            for (var i = 0; i < length; i++) {
                var requestLength = br.ReadUInt16();
                var uid = br.ReadUInt32();
                var leaderboardId = br.ReadUInt32();
                var requestData = br.ReadBytes(requestLength);

                var request = CalcRequest.Decode(requestData);
                
                var discs = request.Discs.Select((d, idx) =>
                    new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
                        d.StatsLevels.Skip(1).Select(p => Stat.SubStats[p.Key] with {
                            Level = p.Value
                        }))).ToArray();

                var result = calc.Calculate(
                    request.AgentId,
                    request.WeaponId,
                    discs,
                    request.Team.ToArray(),
                    request.Rotation.ToArray(),
                    new NotoriousDullahan {
                        StunMultiplier = request.StunBonus
                    }
                );

                var resultData = result.Encode();
                
                bw.Write((uint)resultData.Length);
                bw.Write(uid);
                bw.Write(leaderboardId);
                bw.Write(resultData);

                if (i % 10000 == 0) {
                    Console.WriteLine($"Processed {i}/{length} requests");
                }
            }

            IsRecalculating = false;
            
            File.Delete(RequestFilePath); // cleanup
            
            return new HttpResponse(HttpStatusCode.OK, ResultFilePath);
        });

        await app.StartAsync();
    }
}