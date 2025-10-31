using System.Text.Json.Serialization;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

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
    /// <summary>
    /// Default Drive Discs sub stats
    /// </summary>
    public static Dictionary<Affix, Stat> SubStats { get; } = new() {
        { Affix.Hp,                 new (Affix.Hp, 112) },
        { Affix.Atk,                new (Affix.Atk, 19) },
        { Affix.Def,                new (Affix.Def, 15) },
        { Affix.HpRatio,            new (Affix.HpRatio, 0.03) },
        { Affix.AtkRatio,           new (Affix.AtkRatio, 0.03) },
        { Affix.DefRatio,           new (Affix.DefRatio, 0.048) },
        { Affix.Pen,                new (Affix.Pen, 9) },
        { Affix.CritRate,           new (Affix.CritRate, 0.024) },
        { Affix.CritDamage,         new (Affix.CritDamage, 0.048) },
        { Affix.AnomalyProficiency, new (Affix.AnomalyProficiency, 9) },
    };
    
    /// <summary>
    /// Default Drive Disc Main Stats at lvl. 15
    /// </summary>
    public static Dictionary<Affix, Stat> Stats { get; } = new() {
        { Affix.Hp,                  new (Affix.Hp, 2200) },
        { Affix.Atk,                 new (Affix.Atk, 316) },
        { Affix.Def,                 new (Affix.Def, 184) },
        { Affix.HpRatio,             new (Affix.HpRatio, 0.3) },
        { Affix.AtkRatio,            new (Affix.AtkRatio, 0.3) },
        { Affix.DefRatio,            new (Affix.DefRatio, 0.48) },
        { Affix.CritRate,            new (Affix.CritRate, 0.24) },
        { Affix.CritDamage,          new (Affix.CritDamage, 0.48) },
        { Affix.AnomalyProficiency,  new (Affix.AnomalyProficiency, 92) },
        { Affix.PenRatio,            new (Affix.PenRatio, 0.24) },
        { Affix.IceDmgBonus,         new (Affix.IceDmgBonus, 0.3) },
        { Affix.FireDmgBonus,        new (Affix.FireDmgBonus, 0.3) },
        { Affix.PhysicalDmgBonus,    new (Affix.PhysicalDmgBonus, 0.3) },
        { Affix.ElectricDmgBonus,    new (Affix.ElectricDmgBonus, 0.3) },
        { Affix.EtherDmgBonus,       new (Affix.EtherDmgBonus, 0.3) },
        { Affix.AnomalyMasteryRatio, new (Affix.AnomalyMasteryRatio, 0.3) },
        { Affix.ImpactRatio,         new (Affix.ImpactRatio, 0.18) },
        { Affix.EnergyRegenRatio,    new (Affix.EnergyRegenRatio, 0.6) },
    }; 
    
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
}