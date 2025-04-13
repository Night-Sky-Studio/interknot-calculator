using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Server;
using Microsoft.AspNetCore.Mvc;

namespace InterknotCalculator;

public static class Program {
    public static async Task Main(string[] args) {
        var calc = new Calculator();
        await calc.Init();
        
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
        
        app.UseCors("AllowAll");
        
        app.MapGet("/", () => Results.Text("Inter-Knot Calculator"));
        app.MapPost("/damage", async (HttpRequest request) => {
            var result = await request.ReadFromJsonAsync<CalcRequest>(SerializerContext.Default.CalcRequest);
            
            if (result is null) {
                return Results.BadRequest("Bad request");
            }

            var discs = result.Discs.Select((d, idx) =>
                new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
                    d.StatsLevels.Skip(1).Select(p => (p.Value, Stat.SubStats[p.Key])))
            );
            
            var actions = calc.Calculate(result.AgentId, result.WeaponId, result.StunBonus, discs, result.Team, result.Rotation);
            
            return Results.Json(new CalcResult {
                PerAction = actions, 
                Total = actions.Sum(action => action.Damage)
            }, SerializerContext.Default.CalcResult);
        });
        
        await app.RunAsync();
    }
}