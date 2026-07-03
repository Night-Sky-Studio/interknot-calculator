using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Server.Models;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using AgentAction = InterknotCalculator.Server.Models.AgentAction;

namespace InterknotCalculator.Server;

public static class Program {
    private static string RequestFilePath { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.request");
    private static string ResultFilePath  { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.result");
    
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
            Console.WriteLine("Got recalculate request");
            if (IsRecalculating) {
                return Results.Text("Another request is already running", "text/plain", statusCode: 400);
            }

            try {
                IsRecalculating = true;

                if (!File.Exists(RequestFilePath)) {
                    return Results.Text("No request file found", "text/plain", statusCode: 400);
                }

                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));

                await using var requestStream = new FileStream(RequestFilePath, FileMode.Open, FileAccess.Read);
                using var streamReader = new MessagePackStreamReader(requestStream);

                var leaderboardCountBytes = await streamReader.ReadAsync(cts.Token)
                                            ?? throw new("No leaderboard count found");
                var leaderboardCount = MessagePackSerializer.Deserialize<int>(leaderboardCountBytes, MessagePackConfig.Options);
                var leaderboards = new Leaderboard[leaderboardCount];
                for (var i = 0; i < leaderboardCount; i++) {
                    var msg = await streamReader.ReadAsync(cts.Token)
                              ?? throw new InvalidDataException("Truncated leaderboard section");
                    leaderboards[i] = MessagePackSerializer.Deserialize<Leaderboard>(msg, MessagePackConfig.Options);
                }

                Console.WriteLine($"Decoded {leaderboardCount} leaderboards");

                await using var fileStream = new FileStream(
                    ResultFilePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, bufferSize: 1 << 20, useAsync: false);
                await using var writer = new BufferedStream(fileStream, 1 << 20); // 1 MB buffer

                var charactersCountBytes = await streamReader.ReadAsync(cts.Token)
                                           ?? throw new InvalidDataException("No characters count found");
                var charactersCount = MessagePackSerializer.Deserialize<int>(charactersCountBytes, MessagePackConfig.Options);

                Console.WriteLine($"Total characters: {charactersCount}");

                for (var i = 0; i < charactersCount; i++) {
                    var msg = await streamReader.ReadAsync(cts.Token)
                              ?? throw new InvalidDataException("Truncated characters section");
                    var character = MessagePackSerializer.Deserialize<Character>(msg, MessagePackConfig.Options);

                    var charLeaderboards = leaderboards
                        .Where(l => l.CharacterId == character.CharacterId)
                        .ToArray();
                    var results = new IKNCResult[charLeaderboards.Length];

                    await Parallel.ForEachAsync(
                        Enumerable.Range(0, charLeaderboards.Length),
                        cts.Token,
                        (index, _) => {
                            var leaderboard = charLeaderboards[index];

                            var discs = character.Discs.Select(d =>
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
                                Mindscape = leaderboard.Mindscape,
                                Rotation = leaderboard.Rotation.ToArray(),
                                StunBonus = leaderboard.StunMultiplier,
                                Team = leaderboard.Team.Select(t =>
                                    new TeamMemberRequest(t.AgentId, t.WeaponId, t.DriveDiscSetId)).ToArray()
                            });

                            results[index] = new IKNCResult(character.Uid, leaderboard.Id, character.BuildId, character.IsPrimary, new(
                                result.FinalStats.BaseStats,
                                result.FinalStats.CalculatedStats,
                                result.PerAction.Select(a => new AgentAction(a.AgentId, a.Name, a.Tag, a.Damage, a.Daze)),
                                result.Total
                            ));
                            return ValueTask.CompletedTask;
                        });

                    foreach (var ikncResult in results) {
                        await MessagePackSerializer.SerializeAsync(writer, ikncResult, MessagePackConfig.Options, cts.Token);
                    }

                    if ((i + 1) % 10000 == 0) {
                        Console.WriteLine($"Processed {i + 1} / {charactersCount} characters");
                    }
                }
                Console.WriteLine("Done processing");

                await writer.FlushAsync(cts.Token);

                return Results.Text(ResultFilePath);
            } finally {
                IsRecalculating = false;
            }
        });
        
        await app.RunAsync();
     }
}