namespace InterknotCalculator.Enums;

public enum Affix {
    Hp, HpRatio,
    Def, DefRatio,
    Atk, AtkRatio,
    CritRate, CritDamage,
    Impact, ImpactRatio,
    Pen, PenRatio,
    AnomalyMastery, AnomalyMasteryRatio,
    AnomalyProficiency, 
    Sheer, SheerRatio,
    
    DmgBonus, ResPen,
    IceDmgBonus, IceResPen,
    FireDmgBonus, FireResPen,
    PhysicalDmgBonus, PhysicalResPen,
    ElectricDmgBonus, ElectricResPen,
    EtherDmgBonus, EtherResPen,
    
    EnergyRegen, EnergyRegenRatio,
    
    Daze, DazeBonus,
    
    Unknown = -1
}