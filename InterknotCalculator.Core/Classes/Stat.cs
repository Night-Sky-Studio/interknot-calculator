using System.Text.Json.Serialization;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public struct Stat {
    [JsonPropertyName("Value")]
    public double BaseValue { get; set; }
    public double Level { get; set; } = 1;
    [JsonIgnore]
    public double Value => BaseValue * Level;
    public Affix Affix { get; set; }
    public IEnumerable<SkillTag>? Tags { get; set; } = null;
    
    [JsonIgnore]
    public SkillTag[] SkillTags => Tags?.ToArray() ?? [];
    
    [JsonConstructor]
    public Stat(Affix affix, double baseValue, double level = 1, IEnumerable<SkillTag>? tags = null) {
        BaseValue = baseValue;
        Level = level;
        Affix = affix;
        Tags = tags?.ToArray() ?? [];
    }
    
    public static implicit operator double(Stat v) {
        return v.Value;
    }
    
    public static Stat operator +(Stat a, Stat b) {
        if (a.Affix != b.Affix)
            throw new ArgumentException("Stats must be of the same affix to be combined");
        return a with {
            BaseValue = a.BaseValue + b.BaseValue
        };
    }

    public override string ToString() => $"Stat({Affix}, {Value})";

    public static class MainStat {
        private static class LevelMultipliers {
            public static double[] B { get; } = Enumerable.Range(0, 10).Select(i => 1 + i / 3d).ToArray();
            public static double[] A { get; } = Enumerable.Range(0, 13).Select(i => 1 + i / 4d).ToArray();
            public static double[] S { get; } = Enumerable.Range(0, 16).Select(i => 1 + i / 5d).ToArray();
            public static double[] Get(Rarity r) => r switch {
                Rarity.B => B,
                Rarity.A => A,
                Rarity.S => S,
                _ => throw new ArgumentOutOfRangeException(nameof(r), r, null)
            };
        }
    
        private static class BaseValues {
            private static Dictionary<Affix, double> Base { get; } = new() {
                [Affix.Hp] = 183.5,
                [Affix.Atk] = 26.5,
                [Affix.Def] = 15.5,
                [Affix.HpRatio] = 0.025,
                [Affix.AtkRatio] = 0.025,
                [Affix.DefRatio] = 0.04,
                [Affix.CritRate] = 0.02,
                [Affix.CritDamage] = 0.04,
                [Affix.AnomalyProficiency] = 7.5,
                [Affix.PenRatio] = 0.02,
                [Affix.IceDmgBonus] = 0.025,
                [Affix.FireDmgBonus] = 0.025,
                [Affix.PhysicalDmgBonus] = 0.025,
                [Affix.ElectricDmgBonus] = 0.025,
                [Affix.EtherDmgBonus] = 0.025,
                [Affix.AnomalyMasteryRatio] = 0.025,
                [Affix.ImpactRatio] = 0.015,
                [Affix.EnergyRegenRatio] = 0.05
            };
            
            private static bool IsFlat(Affix a) => 
                a is Affix.Hp or Affix.Atk or Affix.Def or Affix.AnomalyProficiency;
            public static double Get(Rarity r, Affix a) => 
                IsFlat(a) 
                    ? a is Affix.AnomalyProficiency // fuck you, mihoyo...
                        ? Math.Ceiling(Base[a] * ((int)r - 1))
                        : Math.Floor(Base[a] * ((int)r - 1))
                    : Base[a] * ((int)r - 1);
        }
    
        public static Stat Get(Rarity rarity, Affix affix, uint level = 0) => 
            new(affix, LevelMultipliers.Get(rarity)[level] * BaseValues.Get(rarity, affix));
    }

    public static class SubStat {
        private static class BaseValues {
            private static Dictionary<Affix, double> Base { get; } = new() {
                [Affix.Hp] = 37.5,
                [Affix.Atk] = 6.5,
                [Affix.Def] = 5,
                [Affix.HpRatio] = 0.01,
                [Affix.AtkRatio] = 0.01,
                [Affix.DefRatio] = 0.016,
                [Affix.Pen] = 3,
                [Affix.CritRate] = 0.008,
                [Affix.CritDamage] = 0.016,
                [Affix.AnomalyProficiency] = 3
            };
            private static bool IsFlat(Affix a) =>
                a is Affix.Hp or Affix.Atk or Affix.Def or Affix.Pen or Affix.AnomalyProficiency;
            public static double Get(Rarity r, Affix a) => 
                IsFlat(a) ? Math.Floor(Base[a] * ((int)r - 1)) : Base[a] * ((int)r - 1);
        }
        
        public static Stat Get(Rarity rarity, Affix affix, uint level = 1) => 
            new(affix, BaseValues.Get(rarity, affix), level);
    }
}