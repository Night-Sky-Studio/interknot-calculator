﻿using System.Text.Json.Serialization;
using InterknotCalculator.Classes.Extensions;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public struct Stat {
    public double Value { get; set; }
    public Affix Affix { get; set; }
    public string? Expression { get; set; }
    public IEnumerable<SkillTag>? Tags { get; set; } = null;
    
    [JsonIgnore]
    public SkillTag[] SkillTags => Tags?.ToArray() ?? [];
    
    public Stat(double value, Affix affix, string? expression = null, IEnumerable<SkillTag>? tags = null) {
        Expression = expression;
        
        if (Expression is not null && value == 0d) {
            Value = Expression.EvaluateExpression();
        } else {
            Value = value;
        }
        
        Affix = affix;
        Tags = tags?.ToArray() ?? [];
    }
    public static Dictionary<Affix, Stat> SubStats { get; } = new() {
        { Affix.Hp,                 new (112, Affix.Hp) },
        { Affix.Atk,                new (19, Affix.Atk) },
        { Affix.Def,                new (15, Affix.Def) },
        { Affix.HpRatio,            new (0.03, Affix.HpRatio) },
        { Affix.AtkRatio,           new (0.03, Affix.AtkRatio) },
        { Affix.DefRatio,           new (0.048, Affix.DefRatio) },
        { Affix.Pen,                new (9, Affix.Pen) },
        { Affix.CritRate,           new (0.024, Affix.CritRate) },
        { Affix.CritDamage,         new (0.048, Affix.CritDamage) },
        { Affix.AnomalyProficiency, new (9, Affix.AnomalyProficiency) },
    };
    public static Dictionary<Affix, Stat> Stats { get; } = new() {
        { Affix.Hp,                  new (2200, Affix.Hp) },
        { Affix.Atk,                 new (316, Affix.Atk) },
        { Affix.Def,                 new (184, Affix.Def) },
        { Affix.HpRatio,             new (0.3, Affix.HpRatio) },
        { Affix.AtkRatio,            new (0.3, Affix.AtkRatio) },
        { Affix.DefRatio,            new (0.48, Affix.DefRatio) },
        { Affix.CritRate,            new (0.24, Affix.CritRate) },
        { Affix.CritDamage,          new (0.48, Affix.CritDamage) },
        { Affix.AnomalyProficiency,  new (92, Affix.AnomalyProficiency) },
        { Affix.PenRatio,            new (0.24, Affix.PenRatio) },
        { Affix.IceDmgBonus,         new (0.3, Affix.IceDmgBonus) },
        { Affix.FireDmgBonus,        new (0.3, Affix.FireDmgBonus) },
        { Affix.PhysicalDmgBonus,    new (0.3, Affix.PhysicalDmgBonus) },
        { Affix.ElectricDmgBonus,    new (0.3, Affix.ElectricDmgBonus) },
        { Affix.EtherDmgBonus,       new (0.3, Affix.EtherDmgBonus) },
        { Affix.AnomalyMasteryRatio, new (0.3, Affix.AnomalyMasteryRatio) },
        { Affix.ImpactRatio,         new (0.18, Affix.ImpactRatio) },
        { Affix.EnergyRegen,         new (0.6, Affix.EnergyRegen) },
    }; 
    
    public static implicit operator double(Stat v) {
        return v.Value;
    }
}