using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Server.Classes;
using Microsoft.AspNetCore.Mvc;

namespace InterknotCalculator.Server;

public static class Program {
    private static string RequestFilePath { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.request");
    
    private static bool IsRecalculating { get; set; } = false;
    
    public static async Task Main(string[] args) {
        // Initialize Resource Manager
        await Resources.Current.Init();
        
        // Create Calculator instance
        var calc = new Calculator();
        
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

            // Convert Drive Discs from request
            var discs = result.Discs.Select((d, idx) =>
                new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
                    d.StatsLevels.Skip(1).Select(p => Stat.SubStats[p.Key] with {
                        Level = p.Value
                    }))).ToArray();
            
            var calcResult = calc.Calculate(result.AgentId, 
                result.WeaponId, discs, result.Team, result.Rotation, new NotoriousDullahan {
                    StunMultiplier = result.StunBonus
                });
            
            return Results.Json(calcResult, SerializerContext.Default.CalcResult);
        });
        
        app.MapPost("/daze", async (HttpRequest request) => {
            // Read request body
            var result = await request.ReadFromJsonAsync<CalcRequest>(SerializerContext.Default.CalcRequest);
            
            if (result is null) {
                return Results.BadRequest("Bad request");
            }

            // Convert Drive Discs from request
            var discs = result.Discs.Select((d, idx) =>
                new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
                    d.StatsLevels.Skip(1).Select(p => Stat.SubStats[p.Key] with {
                        Level = p.Value
                    }))).ToArray();
            
            var calcResult = calc.Calculate(result.AgentId, 
                result.WeaponId, discs, result.Team, result.Rotation, new NotoriousDullahan(), 
                CalculationType.Daze);
            
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

            using var writer = new ResultWriter();
            
            using var stream = new FileStream(RequestFilePath, FileMode.Open, FileAccess.Read);
            RequestReader.Read(stream, (leaderboards, character) => {
                foreach (var leaderboard in leaderboards) {
                    var discs = character.Discs.Select((d, idx) =>
                        new DriveDisc(d.SetId, Convert.ToUInt32(idx), (Rarity)d.Rarity, Stat.Stats[(Affix)d.MainStat.Affix],
                            d.SubStats.Select(p => Stat.SubStats[(Affix)p.Affix] with {
                                Level = p.Level
                            }))).ToArray();
                    
                    var result = calc.Calculate(leaderboard.CharacterId, leaderboard.WeaponId, discs,
                        leaderboard.TeamIds, leaderboard.Rotation, new NotoriousDullahan {
                            StunMultiplier = leaderboard.StunMultiplier / 1000d
                        });

                    writer.WriteResult(character.Uid, leaderboard.Id, result);

                    if (writer.Count % 10000 == 0) {
                        Console.WriteLine($"Processed {writer.Count} characters");
                    }
                }
            });
            
            IsRecalculating = false;
            
            return Results.Text(writer.Finish());
        });
        
        await app.RunAsync();
     }
}