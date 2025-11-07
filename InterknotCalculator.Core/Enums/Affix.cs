namespace InterknotCalculator.Core.Enums;

public enum Affix {
    Hp, HpRatio,
    Def, DefRatio,
    Atk, AtkRatio, CombatAtkRatio,
    CritRate, CritDamage,
    Impact, ImpactRatio,
    Pen, PenRatio,
    AnomalyMastery, AnomalyMasteryRatio,
    AnomalyProficiency, 
    Sheer, 
    
    DmgBonus, ResPen,
    IceDmgBonus, IceResPen,
    FireDmgBonus, FireResPen,
    PhysicalDmgBonus, PhysicalResPen,
    ElectricDmgBonus, ElectricResPen,
    EtherDmgBonus, EtherResPen,
    DisorderDmgBonus,
    
    SheerBonus,
    IceSheerBonus, 
    FireSheerBonus, 
    PhysicalSheerBonus, 
    ElectricSheerBonus, 
    EtherSheerBonus,
    
    EnergyRegen, EnergyRegenRatio,
    
    Daze, DazeBonus,
    
    AnomalyBuildupBonus, AnomalyBuildupRes,
    
    Unknown = -1
}