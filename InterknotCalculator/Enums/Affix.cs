namespace InterknotCalculator.Enums;

public enum Affix {
    Hp, HpRatio,
    Def, DefRatio,
    Atk, AtkRatio, CombatAtkRatio,
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
    DisorderDmgBonus,
    
    EnergyRegen, EnergyRegenRatio,
    
    Daze, DazeBonus,
    
    AnomalyBuildupBonus, AnomalyBuildupRes,
    
    Unknown = -1
}