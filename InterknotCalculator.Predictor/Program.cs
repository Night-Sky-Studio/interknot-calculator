using System.Collections.Concurrent;
using System.Collections.Immutable;
using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Predictor;

using DiscStat = (uint Level, Stat Stat);

public static class Program {
    private static Random Random { get; } = new();
    private static Calculator Calculator { get; } = new();
    /*
    private static DriveDisc CreateDriveDisc(Affix mainStat, Dictionary<Affix, uint> subStats) {
        var main = Stat.Stats[mainStat];
        var sub = subStats.Select(kvp => {
            var stat = Stat.SubStats[kvp.Key];
            return (kvp.Value, stat with {
                Value = stat.Value * kvp.Value
            });
        });
        var disc = new DriveDisc(
            0,
            0,
            Rarity.S, 
            main, 
            sub
        );
        return disc;
    }
    
    private static DriveDisc GenerateDriveDisc(Affix mainStat, IEnumerable<Affix> affixes) {
        var subStats = affixes.Select(a => (a, 1u))
            .ToDictionary(kvp => kvp.a, kvp => kvp.Item2);
        for (var i = 0; i < 10; i++) {
            if (subStats.Sum(kvp => kvp.Value) == 9 || subStats.Any(ss => ss.Value == 6)) continue;
            subStats[subStats.ElementAt(Random.Next(subStats.Count)).Key] += 1;
        }
        return CreateDriveDisc(mainStat, subStats);
    }
    */
    
    private static DriveDisc CreateDriveDisc(Affix mainStat, List<(Affix affix, uint count)> subStats)
    {
        var main = Stat.Stats[mainStat];

        // Prepare sub stats array without LINQ or "with"
        var subs = new List<(uint, Stat)>(subStats.Count);
        foreach (var (affix, count) in subStats)
        {
            var stat = Stat.SubStats[affix];
            stat.Value *= count; // modify copy
            subs.Add((count, stat));
        }

        return new DriveDisc(0, 0, Rarity.S, main, subs);
    }

    private static DriveDisc GenerateDriveDisc(Affix mainStat, ReadOnlySpan<Affix> affixes)
    {
        // Initialize subStats as a list of tuples (no dictionary needed)
        var subStats = new List<(Affix, uint)>(affixes.Length);
        foreach (var affix in affixes)
            subStats.Add((affix, 1));

        // Random upgrades
        for (var i = 0; i < 10; i++)
        {
            uint total = 0;
            bool hasSix = false;
            for (int j = 0; j < subStats.Count; j++)
            {
                total += subStats[j].Item2;
                if (subStats[j].Item2 == 6)
                    hasSix = true;
            }

            if (total == 9 || hasSix) continue;

            int index = Random.Next(subStats.Count);
            var (affix, value) = subStats[index];
            subStats[index] = (affix, value + 1);
        }

        return CreateDriveDisc(mainStat, subStats);
    }
    
    
    private static ImmutableList<Affix> Possible4DiscStats { get; } = [
        Affix.HpRatio,
        Affix.AtkRatio,
        Affix.DefRatio,
        Affix.CritRate,
        Affix.CritDamage,
        Affix.AnomalyProficiency,
    ];    
    private static ImmutableList<Affix> Possible5DiscStats { get; } = [
        Affix.HpRatio,
        Affix.AtkRatio,
        Affix.DefRatio,
        Affix.PenRatio,
        Affix.IceDmgBonus,
        Affix.FireDmgBonus,
        Affix.PhysicalDmgBonus,
        Affix.ElectricDmgBonus,
        Affix.EtherDmgBonus
    ];
    private static ImmutableList<Affix> Possible6DiscStats { get; } = [
        Affix.HpRatio,
        Affix.AtkRatio,
        Affix.DefRatio,
        Affix.AnomalyMasteryRatio,
        Affix.ImpactRatio,
        Affix.EnergyRegenRatio
    ];
    
    private static Affix GenerateMainStatAffix(uint slot) {
        return slot switch {
            1 => Affix.Hp,
            2 => Affix.Atk,
            3 => Affix.Def,
            4 => Possible4DiscStats[Random.Next(Possible4DiscStats.Count)],
            5 => Possible5DiscStats[Random.Next(Possible5DiscStats.Count)],
            6 => Possible6DiscStats[Random.Next(Possible6DiscStats.Count)],
            _ => throw new ArgumentOutOfRangeException(nameof(slot), "Slot must be between 0 and 5")
        };
    }
    
    const int SimulationsCount = 1_000_000_000;
    const int MaxParallelTasks = -1;

    const int BatchSize = 1000;
    
    public static async Task Main() {
        await Resources.Current.Init();

        var results = new BlockingCollection<(IEnumerable<DriveDisc>, CalcResult)>();

        List<uint> team = [];
        List<string> rotation = ["lingering_snow"];
        
        Parallel.For(0, (SimulationsCount + BatchSize - 1) / BatchSize, new() {
            MaxDegreeOfParallelism = MaxParallelTasks
        },batchIdx => {
            int start = batchIdx * BatchSize;
            int end = Math.Min(start + BatchSize, SimulationsCount);
            for (var i = start; i < end; i++) {
                List<DriveDisc> discs = [
                    GenerateDriveDisc(Affix.Hp, [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen]),
                    GenerateDriveDisc(Affix.Atk, [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen]),
                    GenerateDriveDisc(Affix.Def, [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen]),
                    GenerateDriveDisc(GenerateMainStatAffix(4), [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen]),
                    GenerateDriveDisc(GenerateMainStatAffix(5), [Affix.AnomalyProficiency, Affix.CritDamage, Affix.AtkRatio, Affix.Pen]),
                    GenerateDriveDisc(GenerateMainStatAffix(6), [Affix.CritRate, Affix.CritDamage, Affix.Atk, Affix.Pen]),
                ];
                results.Add((discs, Calculator.Calculate(1091, 14109, discs, team, rotation, new NotoriousDullahan())));
            }
            Console.WriteLine($"[PROGRESS] {results.Count} / {SimulationsCount} ({(double)results.Count / SimulationsCount:P2})");
        });

        Console.WriteLine("Result\n=====================\n");
        Console.WriteLine($"Count: {results.Count}\n");
        Console.WriteLine("Top 10\n=====================\n");
        foreach (var (discs, result) in results.OrderByDescending(r => r.Item2.Total).Take(10)) {
            Console.WriteLine($"Total: {result.Total:0.###}\n{string.Join("\n", discs)}\n");
        }
        
        // for (var x = 0; x < 100; x++) {
        //     var disc = GenerateDriveDisc(Affix.PenRatio, [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen]);
        //     Console.WriteLine($"Disc {x + 1, -8} {disc}");
        // }

        // CheckMainStats(1091, 14109,[
        //     "hisetsu 1",
        //     "hisetsu 2",
        //     "kazahana 3",
        //     "kazahana 4",
        //     "kazahana 5",
        //     "shimotsuki 3",
        //     "springs_call",
        //     "lingering_snow",
        //     "shimotsuki 3"
        // ]);
    }
}