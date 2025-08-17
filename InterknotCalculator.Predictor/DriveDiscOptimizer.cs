using System.Diagnostics;
using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Predictor;

// Optimizes Drive Disc combinations for Miyabi (agentId 1091) using Calculator.
// Enhancements over previous version:
//  - Uses Calculator.Calculate(...) for more accurate total damage (supports multi-step rotations).
//  - Considers specified best set patterns (4+2 and 2+2+2) among sets: 32700, 31000, 32800.
//  - Thread-safe: each worker uses its own Calculator instance (no global locking needed).
//  - Beam search for per-slot substat level allocation (kept small to control combinatorics).
//  - Progress reporting (count-based).
public static class DriveDiscOptimizer {
    // Agent / weapon / rotation constants
    private const int AgentId = 1091; // Miyabi
    private const int WeaponId = 14109;
    private static readonly uint[] Team = [];
    private static readonly string[] Rotation = [
        "hisetsu 1",
        "hisetsu 2",
        "kazahana 3",
        "kazahana 4",
        "kazahana 5",
        "shimotsuki 3",
        "springs_call",
        "lingering_snow",
        "shimotsuki 3"
    ];

    // Set IDs (best for Miyabi per user input)
    private const uint Set32700 = 32700;
    private const uint Set31000 = 31000;
    private const uint Set32800 = 32800;

    // Patterns to evaluate: only counts matter (slot assignment irrelevant if set bonuses depend solely on counts)
    private readonly record struct SetPattern(string Name, int C32700, int C31000, int C32800);

    private static readonly SetPattern[] Patterns = [
        new SetPattern("4pc 32700 + 2pc 31000", 4, 2, 0),
        new SetPattern("4pc 32700 + 2pc 32800", 4, 0, 2),
        new SetPattern("2pc 32700 + 2pc 31000 + 2pc 32800", 2, 2, 2),
    ];

    // Beam widths (tune for speed vs accuracy)
    private const int BeamWidthFixed = 6;     // Slots with one main stat choice
    private const int BeamWidthVariable = 10; // Slots 4 & 5 (aggregate across all main stats)
    private const int ProgressInterval = 50_000;
    private const int TopN = 10;

    // Slots (1..6)
    private static readonly uint[] AllSlots = [1, 2, 3, 4, 5, 6];

    // Main stat mapping per slot
    private static readonly Dictionary<uint, Affix[]> SlotMainStats = new() {
        {
            1u, [Affix.Hp]
        }, {
            2u, [Affix.Atk]
        }, {
            3u, [Affix.Def]
        }, {
            4u, [
                Affix.AtkRatio,
                Affix.CritRate,
                Affix.CritDamage,
                Affix.AnomalyProficiency
            ]
        }, {
            5u, [
                Affix.AtkRatio,
                Affix.PenRatio,
                Affix.IceDmgBonus
            ]
        }, {
            6u, [Affix.AtkRatio]
        },
    };

    // Global sub-stat priority
    private static readonly Affix[] SubStatPriority = [
        Affix.CritRate,
        Affix.CritDamage,
        Affix.AtkRatio,
        Affix.Pen,
        Affix.Atk,
        Affix.AnomalyProficiency
    ];

    private sealed class DiscVariant {
        public uint Slot { get; init; }
        public Affix MainAffix { get; init; }
        public Stat MainStat { get; init; }
        public (Affix affix, int level)[] SubStats { get; init; } = [];
        public double EstDamage { get; init; } // damage when evaluated during beam search (with baseline discs)
        public override string ToString() {
            var subs = string.Join(", ", SubStats.Select(s => $"{s.affix}(Lv{s.level})"));
            return $"Slot {Slot} Main {MainAffix} [{subs}]";
        }
    }

    private static readonly Lock Lock = new();
    
    private sealed class TopResults {
        
        private readonly List<(DriveDisc[] discs, double dmg)> _list = [];

        public void Consider(DriveDisc[] discs, double damage) {
            lock (Lock) {
                if (_list.Count < TopN || damage >= _list.Min(r => r.dmg)) {
                    _list.Add((Clone(discs), damage));
                    _list.Sort((a, b) => b.dmg.CompareTo(a.dmg));

                    // Allow ties; mild trimming if runaway
                    if (_list.Count > TopN * 4) {
                        double cutoff = _list[Math.Min(TopN - 1, _list.Count - 1)].dmg;
                        _list.RemoveAll(r => r.dmg < cutoff);
                        if (_list.Count > TopN * 2)
                            _list.RemoveRange(TopN, _list.Count - TopN);
                    }
                }
            }
        }

        public IReadOnlyList<(DriveDisc[] discs, double dmg)> Snapshot() {
            lock (Lock) {
                return _list
                    .OrderByDescending(r => r.dmg)
                    .Take(TopN)
                    .Select(r => (Clone(r.discs), r.dmg))
                    .ToArray();
            }
        }

        private static DriveDisc[] Clone(DriveDisc[] src) {
            var copy = new DriveDisc[src.Length];
            Array.Copy(src, copy, src.Length);
            return copy;
        }
    }

    private static Calculator Calculator { get; } = new();
    
    public static void Run() {
        // Enemy can be shared (no internal mutation in this scenario)
        var enemy = new NotoriousDullahan();

        // 1. Baseline discs (first main stat option per slot, substats level 1)
        var baselineDiscs = BuildBaselineDiscs();

        // Evaluate baseline (no sets yet, SetIds=0)
        double baselineTotal = Evaluate(Calculator, baselineDiscs, enemy);

        // 3. Generate disc variants per slot (beam search per main stat)
        var perSlotVariants = new Dictionary<uint, DiscVariant[]>();

        foreach (var slot in AllSlots) {
            var variants = new List<DiscVariant>();
            foreach (var main in SlotMainStats[slot]) {
                var gen = GenerateDiscVariantsForSlot(
                    slot,
                    main,
                    baselineDiscs,
                    baselineTotal,
                    Calculator,
                    enemy,
                    beamWidth: SlotMainStats[slot].Length == 1 ? BeamWidthFixed : BeamWidthVariable
                );
                variants.AddRange(gen);
            }

            // Keep strongest variants (aggregate)
            int keep = SlotMainStats[slot].Length == 1 ? BeamWidthFixed : BeamWidthVariable;
            perSlotVariants[slot] = variants
                .OrderByDescending(v => v.EstDamage)
                .Take(keep)
                .ToArray();
        }

        // Compute combination count (before set patterns)
        long baseCombos = perSlotVariants.Values.Aggregate(1L, (acc, arr) => acc * arr.Length);
        long totalCombos = baseCombos * Patterns.Length;
        Console.WriteLine($"Disc variant combinations: {baseCombos:N0}");
        Console.WriteLine($"Total evaluations with set patterns: {totalCombos:N0}");

        // 4. Parallel combination enumeration
        var slotOrder = AllSlots;
        var variantArrays = slotOrder.Select(s => perSlotVariants[s]).ToArray();

        var top = new TopResults();
        long evaluated = 0;
        var sw = Stopwatch.StartNew();

        var firstSlotVariants = variantArrays[0];

        Parallel.For(0, firstSlotVariants.Length, i => {
            var discs = new DriveDisc[6];
            IterateSlots(
                slotIndex: 0,
                firstIndex: i,
                variantArrays: variantArrays,
                discs: discs,
                enemy: enemy,
                calculator: Calculator,
                top: top,
                ref evaluated,
                sw: sw
            );
        });

        // 5. Output
        var results = top.Snapshot();

        Console.WriteLine();
        Console.WriteLine("Result");
        Console.WriteLine("=====================\n");
        Console.WriteLine($"Count: {evaluated:N0}\n");
        Console.WriteLine("Top 10:");
        Console.WriteLine("=====================\n");

        foreach (var (discs, dmg) in results) {
            Console.WriteLine($"Total: {dmg}");
            Console.WriteLine("Discs:");
            foreach (var d in discs.OrderBy(d => d.Slot)) {
                Console.WriteLine(d);
            }
            Console.WriteLine();
        }
    }

    // Recursive enumeration of slot variants; after building the 6-disc variant set (with SetId=0)
    // we apply each set pattern (assign SetIds) and evaluate.
    private static void IterateSlots(
        int slotIndex,
        int firstIndex,
        DiscVariant[][] variantArrays,
        DriveDisc[] discs,
        NotoriousDullahan enemy,
        Calculator calculator,
        TopResults top,
        ref long evaluated,
        Stopwatch sw) {
        if (slotIndex == variantArrays.Length) {
            // We have 6 variant discs with SetId=0 (stat-only). Evaluate each pattern.
            foreach (var pattern in Patterns) {
                var patternDiscs = ApplyPattern(discs, pattern);
                var result = Evaluate(calculator, patternDiscs, enemy);

                top.Consider(patternDiscs, result);

                long count = Interlocked.Increment(ref evaluated);
                if (count % ProgressInterval == 0) {
                    Console.WriteLine($"Evaluated: {count:N0}  Elapsed: {sw.Elapsed}");
                }
            }
            return;
        }

        var variants = variantArrays[slotIndex];
        int start = slotIndex == 0 ? firstIndex : 0;
        int end = slotIndex == 0 ? firstIndex + 1 : variants.Length;

        for (int i = start; i < end; i++) {
            var v = variants[i];
            discs[slotIndex] = BuildDiscFromVariant(v);
            IterateSlots(slotIndex + 1, firstIndex, variantArrays, discs, enemy, calculator, top, ref evaluated, sw);
        }
    }

    private static DriveDisc BuildDiscFromVariant(DiscVariant v) {
        var subStats = v.SubStats.Select(s =>
            new Stat(s.affix, Stat.SubStats[s.affix].BaseValue, (uint)s.level)).ToArray();

        return new DriveDisc(
            SetId: 0,
            Slot: v.Slot,
            Rarity: Rarity.S,
            MainStat: v.MainStat,
            SubStats: subStats
        );
    }

    private static DriveDisc[] ApplyPattern(DriveDisc[] baseDiscs, SetPattern pattern) {
        // Since only counts matter, assign deterministically by slot order.
        var sorted = baseDiscs.OrderBy(d => d.Slot).ToArray();
        var copy = new DriveDisc[sorted.Length];

        int idx = 0;

        void Assign(int count, uint setId) {
            for (int c = 0; c < count; c++) {
                var d = sorted[idx++];
                copy[idx - 1] = new DriveDisc(
                    SetId: setId,
                    Slot: d.Slot,
                    Rarity: d.Rarity,
                    MainStat: d.MainStat,
                    SubStats: d.SubStats
                );
            }
        }

        Assign(pattern.C32700, Set32700);
        Assign(pattern.C31000, Set31000);
        Assign(pattern.C32800, Set32800);

        // Any remaining (should not happen with defined patterns) default to 0
        while (idx < sorted.Length) {
            var d = sorted[idx];
            copy[idx] = new DriveDisc(
                SetId: 0,
                Slot: d.Slot,
                Rarity: d.Rarity,
                MainStat: d.MainStat,
                SubStats: d.SubStats
            );
            idx++;
        }

        return copy;
    }

    private static double Evaluate(
        Calculator calculator,
        DriveDisc[] discs,
        NotoriousDullahan enemy) {
        lock (Lock) {
            var result = calculator.Calculate(
                AgentId,
                WeaponId,
                discs,
                Team,
                Rotation,
                enemy
            );
            return result.Total;
        }
    }

    private static DiscVariant[] GenerateDiscVariantsForSlot(
        uint slot,
        Affix mainAffix,
        DriveDisc[] baselineDiscs,
        double baselineTotal,
        Calculator calculator,
        NotoriousDullahan enemy,
        int beamWidth) {
        var mainStat = Stat.Stats[mainAffix];
        var subAffixes = SelectSubAffixes(mainAffix);
        int subCount = subAffixes.Length;

        // Base levels (all 1)
        var baseLevels = subAffixes.Select(_ => 1).ToArray();

        // Build baseline variant discs for this slot (clone)
        var working = CloneDiscs(baselineDiscs);
        int slotIndex = Array.FindIndex(working, d => d.Slot == slot);
        if (slotIndex < 0) slotIndex = (int)slot - 1;

        var baseDisc = new DriveDisc(
            SetId: 0,
            Slot: slot,
            Rarity: Rarity.S,
            MainStat: mainStat,
            SubStats: subAffixes.Select(a => new Stat(a, Stat.SubStats[a].BaseValue, 1)).ToArray()
        );
        working[slotIndex] = baseDisc;
        double baseSlotDamage = Evaluate(calculator, working, enemy);

        var frontier = new List<(int[] levels, double dmg)>();
        var allStates = new List<(int[] levels, double dmg)>();
        var visited = new HashSet<string>();

        void AddState(int[] levels) {
            string key = Key(levels);
            if (visited.Contains(key)) return;
            if (ExtraUsed(levels) > 5) return;
            if (levels.Any(l => l < 1 || l > 6)) return;

            // Replace disc with these levels
            var disc = new DriveDisc(
                SetId: 0,
                Slot: slot,
                Rarity: Rarity.S,
                MainStat: mainStat,
                SubStats: subAffixes.Select((a, idx) =>
                    new Stat(a, Stat.SubStats[a].BaseValue, (uint)levels[idx])).ToArray()
            );
            working[slotIndex] = disc;
            double dmg = Evaluate(calculator, working, enemy);

            frontier.Add((CloneLevels(levels), dmg));
            allStates.Add((CloneLevels(levels), dmg));
            visited.Add(key);
        }

        AddState(baseLevels);

        // Beam expand up to 5 extra levels
        for (int depth = 0; depth < 5; depth++) {
            // Select states with exactly 'depth' extras to expand
            var toExpand = allStates.Where(s => ExtraUsed(s.levels) == depth).ToArray();
            if (toExpand.Length == 0) break;

            foreach (var state in toExpand) {
                for (int i = 0; i < subCount; i++) {
                    if (state.levels[i] >= 6) continue;
                    var next = CloneLevels(state.levels);
                    next[i]++;
                    AddState(next);
                }
            }

            // Beam prune
            frontier = frontier
                .OrderByDescending(s => s.dmg)
                .Take(beamWidth)
                .ToList();
            allStates = frontier.ToList();
        }

        // Convert to DiscVariant
        var variants = frontier
            .OrderByDescending(s => s.dmg)
            .Select(s => {
                var subs = subAffixes.Select((a, idx) => (a, s.levels[idx])).ToArray();
                return new DiscVariant {
                    Slot = slot,
                    MainAffix = mainAffix,
                    MainStat = mainStat,
                    SubStats = subs,
                    EstDamage = s.dmg
                };
            })
            .ToArray();

        return variants;
    }

    private static int ExtraUsed(int[] levels) => levels.Sum(l => l - 1);

    private static int[] CloneLevels(int[] src) {
        var c = new int[src.Length];
        Array.Copy(src, c, src.Length);
        return c;
    }

    private static string Key(int[] levels) => string.Join(",", levels);

    private static Affix[] SelectSubAffixes(Affix main) {
        var list = new List<Affix>(4);
        foreach (var a in SubStatPriority) {
            if (a == main) continue;
            if (!Stat.SubStats.ContainsKey(a)) continue;
            list.Add(a);
            if (list.Count == 4) break;
        }
        return list.ToArray();
    }

    private static DriveDisc[] BuildBaselineDiscs() {
        var discs = new List<DriveDisc>(6);
        foreach (var slot in AllSlots) {
            var main = SlotMainStats[slot][0]; // first candidate as baseline
            var mainStat = Stat.Stats[main];
            var subs = SelectSubAffixes(main)
                .Select(a => new Stat(a, Stat.SubStats[a].BaseValue, 1))
                .ToArray();

            discs.Add(new DriveDisc(
                SetId: 0,
                Slot: slot,
                Rarity: Rarity.S,
                MainStat: mainStat,
                SubStats: subs
            ));
        }
        return discs.OrderBy(d => d.Slot).ToArray();
    }

    private static DriveDisc[] CloneDiscs(DriveDisc[] discs) {
        var clone = new DriveDisc[discs.Length];
        Array.Copy(discs, clone, discs.Length);
        return clone;
    }
}