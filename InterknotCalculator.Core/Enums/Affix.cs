using System.Diagnostics.CodeAnalysis;

namespace InterknotCalculator.Core.Enums;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Affix {
    Unknown = -1,

    // Additive affixes
    Hp, Def, Atk, Impact, Pen,
    AnomalyMastery, AnomalyProficiency,
    EnergyRegen, SheerForce, Daze,
    _Additive,

    // Multiplicative affixes
    HpRatio, DefRatio, AtkRatio, CombatAtkRatio, ImpactRatio, PenRatio,
    AnomalyMasteryRatio,
    CritRate, CritDamage,
    EnergyRegenRatio, SheerForceBonus, DazeBonus,

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

    _Multiplicative,
}

public static class AffixExtensions {
    public static bool IsMultiplicative(this Affix a) => 
        a is > Affix._Additive and < Affix._Multiplicative;
}