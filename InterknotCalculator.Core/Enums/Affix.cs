using System.Diagnostics.CodeAnalysis;

namespace InterknotCalculator.Core.Enums;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Affix {
    Unknown = -1,

    // Flat affixes
    Hp, Def, Atk, Impact, Pen,
    AnomalyMastery, AnomalyProficiency,
    EnergyRegen, SheerForce, Daze,
    
    // Treated as flat affixes, despite their names
    CritRate, CritDamage, PenRatio,
    SheerForceBonus, DazeBonus,
    
    DmgBonus, ResPen,
    IceDmgBonus, IceResPen,
    FireDmgBonus, FireResPen,
    PhysicalDmgBonus, PhysicalResPen,
    ElectricDmgBonus, ElectricResPen,
    EtherDmgBonus, EtherResPen,
    WindDmgBonus, WindResPen,
    DisorderDmgBonus, AnomalyDmgBonus,

    IceSheerBonus, 
    FireSheerBonus, 
    PhysicalSheerBonus, 
    ElectricSheerBonus, 
    EtherSheerBonus,

    AnomalyBuildupBonus, AnomalyBuildupRes,
    
    _Flat,

    // Ratio affixes - apply as `Base * (1 + Sum(Ratio))`
    HpRatio, DefRatio, AtkRatio, CombatAtkRatio, ImpactRatio,
    AnomalyMasteryRatio, EnergyRegenRatio, 

    _Ratio,
}

public static class AffixExtensions {
    public static bool IsRatio(this Affix a) => 
        a is > Affix._Flat and < Affix._Ratio;

    public static Affix Flat(this Affix a) => a switch {
        Affix.HpRatio => Affix.Hp,
        Affix.AtkRatio => Affix.Atk,
        Affix.CombatAtkRatio => Affix.Atk,
        Affix.DefRatio => Affix.Def,
        Affix.ImpactRatio => Affix.Impact,
        Affix.AnomalyMasteryRatio => Affix.AnomalyMastery,
        Affix.EnergyRegenRatio => Affix.EnergyRegen,
        _ => a
    };
}