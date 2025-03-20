using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Server;
using Microsoft.AspNetCore.Mvc;

namespace InterknotCalculator;

public static class Program {
    public static async Task Main(string[] args) {
        var calc = new Calculator();
        await calc.Init();

        /*
        var miyabi = calc.Calculate(1091, 14109, [
            new(31000, 1, Rarity.S, Stat.Stats[Affix.Hp], [
                (2, Stat.SubStats[Affix.HpRatio]),
                (1, Stat.SubStats[Affix.CritDamage]),
                (3, Stat.SubStats[Affix.CritRate]),
                (3, Stat.SubStats[Affix.AtkRatio])
            ]),
            new(32700, 2, Rarity.S, Stat.Stats[Affix.Atk], [
                (3, Stat.SubStats[Affix.AnomalyProficiency]),
                (1, Stat.SubStats[Affix.AtkRatio]),
                (3, Stat.SubStats[Affix.CritDamage]),
                (2, Stat.SubStats[Affix.CritRate])
            ]),
            new(31000, 3, Rarity.S, Stat.Stats[Affix.Def], [
                (1, Stat.SubStats[Affix.CritRate]),
                (3, Stat.SubStats[Affix.AnomalyProficiency]),
                (3, Stat.SubStats[Affix.CritDamage]),
                (1, Stat.SubStats[Affix.Pen])
            ]),
            new(32700, 4, Rarity.S, Stat.Stats[Affix.CritDamage], [
                (2, Stat.SubStats[Affix.Hp]),
                (2, Stat.SubStats[Affix.HpRatio]),
                (4, Stat.SubStats[Affix.CritRate]),
                (1, Stat.SubStats[Affix.Pen])
            ]),
            new(32700, 5, Rarity.S, Stat.Stats[Affix.AtkRatio], [
                (2, Stat.SubStats[Affix.HpRatio]),
                (3, Stat.SubStats[Affix.CritRate]),
                (2, Stat.SubStats[Affix.CritDamage]),
                (1, Stat.SubStats[Affix.AnomalyProficiency])
            ]),
            new(32700, 6, Rarity.S, Stat.Stats[Affix.AtkRatio], [
                (2, Stat.SubStats[Affix.Hp]),
                (2, Stat.SubStats[Affix.CritDamage]),
                (3, Stat.SubStats[Affix.Atk]),
                (1, Stat.SubStats[Affix.CritRate])
            ])
        ], [
            "springs_call 1", "lingering_snow 1", "frostburn", "shimotsuki 3", "shatter"
        ]);

        Console.WriteLine(string.Join("\n", miyabi.PerAction));
        Console.WriteLine($"Total: {miyabi.Total}");
        */

        
        // var janeDoe = calc.Calculate(1261, 14126, [
        //     new(31300, 1, Rarity.S, Stat.Stats[Affix.Hp], [
        //         (3, Stat.SubStats[Affix.AnomalyProficiency]),
        //         (2, Stat.SubStats[Affix.HpRatio]),
        //         (1, Stat.SubStats[Affix.DefRatio]),
        //         (3, Stat.SubStats[Affix.CritDamage])
        //     ]),
        //     new(31300, 2, Rarity.S, Stat.Stats[Affix.Atk], [
        //         (3, Stat.SubStats[Affix.AnomalyProficiency]),
        //         (1, Stat.SubStats[Affix.HpRatio]),
        //         (3, Stat.SubStats[Affix.AtkRatio]),
        //         (1, Stat.SubStats[Affix.CritRate])
        //     ]),
        //     new(32600, 3, Rarity.S, Stat.Stats[Affix.Def], [
        //         (1, Stat.SubStats[Affix.CritDamage]),
        //         (3, Stat.SubStats[Affix.AtkRatio]),
        //         (2, Stat.SubStats[Affix.AnomalyProficiency]),
        //         (2, Stat.SubStats[Affix.Atk])
        //     ]),
        //     new(32600, 4, Rarity.S, Stat.Stats[Affix.AnomalyProficiency], [
        //         (4, Stat.SubStats[Affix.CritRate]),
        //         (2, Stat.SubStats[Affix.CritDamage]),
        //         (1, Stat.SubStats[Affix.Hp]),
        //         (2, Stat.SubStats[Affix.HpRatio])
        //     ]),
        //     new(32600, 5, Rarity.S, Stat.Stats[Affix.PhysicalDmgBonus], [
        //         (1, Stat.SubStats[Affix.CritDamage]),
        //         (2, Stat.SubStats[Affix.HpRatio]),
        //         (2, Stat.SubStats[Affix.AnomalyProficiency]),
        //         (3, Stat.SubStats[Affix.Pen])
        //     ]),
        //     new(32600, 6, Rarity.S, Stat.Stats[Affix.AnomalyMasteryRatio], [
        //         (2, Stat.SubStats[Affix.HpRatio]),
        //         (2, Stat.SubStats[Affix.CritRate]),
        //         (3, Stat.SubStats[Affix.AtkRatio]),
        //         (1, Stat.SubStats[Affix.DefRatio])
        //     ]),
        // ], [
        //     "flowers_of_sin", "phantom_thrust", "salchow_jump 1", "assault", "salchow_jump 2",
        //     "aerial_sweep_cleanout", "aerial_sweep_cleanout", "assault",
        //     "phantom_thrust", "final_curtain", "assault",
        //     "salchow_jump 1", "salchow_jump 2"
        // ]);
        //
        // Console.WriteLine(string.Join('\n', janeDoe.PerAction));
        // Console.WriteLine($"Total: {janeDoe.Total}");

        
        var builder = WebApplication.CreateSlimBuilder(args);
        
        builder.Services.AddCors(options => {
            options.AddPolicy("AllowAll",
                policy => {
                    policy.AllowAnyOrigin()   // Allow requests from any origin
                        .AllowAnyMethod()   // Allow any HTTP method (GET, POST, etc.)
                        .AllowAnyHeader();  // Allow any headers
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
            
            var calcResult = calc.Calculate(result.AgentId, result.WeaponId, discs, result.Rotation);
            
            return Results.Json(new CalcResult {
                PerAction = calcResult.PerAction.ToArray(), 
                Total = calcResult.Total
            }, SerializerContext.Default.CalcResult);
        });
        
        await app.RunAsync();
    }
}