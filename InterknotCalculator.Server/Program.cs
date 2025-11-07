using System.Net.WebSockets;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace InterknotCalculator;

public static class Program {
    public static async Task Main(string[] args) {
        // Initialize Resource Manager
        await Resources.Current.Init();
        
        // Create Calculator instance
        var calc = new Calculator();
        
        // Create web app
        var builder = WebApplication.CreateSlimBuilder(args);
        builder.WebHost.UseUrls("http://localhost:5101/");
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
                result.WeaponId, discs, result.Team, result.Rotation, new NotoriousDullahan(),
                CalculationType.Damage);
            
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

        app.Map("/ws/damage", async context => {
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[4096];

            while (true) {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close) {
                    break;
                }
                
                var request = CalcRequest.Decode(buffer.AsSpan(0, result.Count));
                
                var discs = request.Discs.Select((d, idx) =>
                    new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
                        d.StatsLevels.Skip(1).Select(p => Stat.SubStats[p.Key] with {
                            Level = p.Value
                        }))).ToArray();
            
                var calcResult = calc.Calculate(request.AgentId, 
                    request.WeaponId, discs, request.Team, request.Rotation, new NotoriousDullahan {
                        StunMultiplier = request.StunBonus
                    });

                await socket.SendAsync(calcResult.Encode(), WebSocketMessageType.Binary, WebSocketMessageFlags.EndOfMessage,
                    CancellationToken.None);
            }
        });
    
        
        await app.RunAsync();
     }
}