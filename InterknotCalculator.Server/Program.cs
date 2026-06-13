using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Server.Models;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using DriveDisc = InterknotCalculator.Core.Classes.DriveDisc;

namespace InterknotCalculator.Server;

public static class Program {
    private static string RequestFilePath { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.request");
    
    private static bool IsRecalculating { get; set; } = false;
    
    public static async Task Main(string[] args) {
        // Initialize Resource Manager
        await Resources.Current.Init();
        
        // Create web app
        var builder = WebApplication.CreateSlimBuilder(args);
        builder.WebHost.UseUrls("http://127.0.0.1:5200/");
        builder.Services.AddCors(options => {
            options.AddPolicy("AllowAll",
                policy => {
                    policy.AllowAnyOrigin()     // Allow requests from any origin
                        .AllowAnyMethod()       // Allow any HTTP method (GET, POST, etc.)
                        .AllowAnyHeader();      // Allow any headers
                });
        });
        builder.Services.Configure<JsonOptions>(opts => {
            opts.JsonSerializerOptions.TypeInfoResolver = SerializerContext.Default;
        });
        
        var app = builder.Build();
        
        // Allow CORS for all requests
        app.UseCors("AllowAll");
        
        app.MapGet("/", () => Results.Text("Inter-Knot Calculator"));
        app.MapPost("/damage", async (HttpRequest request) => {
            // Read request body
            var result = await request.ReadFromJsonAsync<CalcRequest>(SerializerContext.Default.CalcRequest);
            
            if (result is null) {
                return Results.BadRequest("Bad request");
            }
            
            var calcResult = Calculator.Calculate(result);
            
            return Results.Json(calcResult, SerializerContext.Default.CalcResult);
        });
        
        app.MapGet("/recalculate", async () => {
            if (IsRecalculating) {
                return Results.Text("Another request is already running", "text/plain", statusCode: 400);
            }
            
            IsRecalculating = true;
            
            if (!File.Exists(RequestFilePath)) {
                return Results.Text("No request file found", "text/plain", statusCode: 400);
            }
            
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            
            await using var requestStream = new FileStream(RequestFilePath, FileMode.Open, FileAccess.Read); 
            using var streamReader = new MessagePackStreamReader(requestStream);

            var leaderboardCount = await streamReader.ReadArrayHeaderAsync(cts.Token);
            var leaderboards = new Leaderboard[leaderboardCount];
            var i = 0;
            await foreach (var lb in streamReader.ReadArrayAsync(cts.Token)) {
                leaderboards[i++] = MessagePackSerializer.Deserialize<Leaderboard>(lb);
            }

            List<byte> resultBytes = new();
            
            while (await streamReader.ReadAsync(cts.Token) is { } message) {
                var character = MessagePackSerializer.Deserialize<Character>(message);
                foreach (var leaderboard in leaderboards) {
                    var discs = character.Discs.Select((d, idx) =>
                        new DriveDiscRequest {
                            SetId = d.SetId,
                            Rarity = d.Rarity,
                            Stats = [(Affix)d.MainStat.Affix, ..d.SubStats.Select(p => (Affix)p.Affix)],
                            Levels = [d.MainStat.Level, ..d.SubStats.Select(p => p.Level)]
                        }).ToArray();
                    
                    var result = Calculator.Calculate(new() {
                        AgentId = leaderboard.CharacterId,
                        WeaponId = leaderboard.WeaponId,
                        Discs = discs,
                        Mindscape = character.Mindscape,
                        Rotation = leaderboard.Rotation.ToArray(),
                        StunBonus = leaderboard.StunMultiplier,
                        Team = leaderboard.Team.Select(t => 
                            new TeamMemberRequest(t.AgentId, t.WeaponId, t.DriveDiscSetId)).ToArray()
                    });
                    
                    // TODO: serialize as IKNCResult
                    resultBytes.AddRange(MessagePackSerializer.Serialize(result));
                }
            }
            
            return Results.File(resultBytes.ToArray(), "application/octet-stream", "results.msgpack");
        });
        
        await app.RunAsync();
     }
}