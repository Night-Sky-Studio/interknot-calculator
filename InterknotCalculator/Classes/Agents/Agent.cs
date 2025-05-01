using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

/// <summary>
/// Base Agent class
/// All agents should inherit from this class and be sealed
/// </summary>
public abstract class Agent {
    public Speciality Speciality { get; set; }
    public Element Element { get; set; }
    public Rarity Rarity { get; set; }
    public Faction Faction { get; set; }
    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    public SafeDictionary<Affix, double> BonusStats { get; set; } = new();
    public SafeDictionary<Affix, double> ExternalBonus { get; set; } = new();
    public List<Stat> TagBonus { get; set; } = [];
    public List<Stat> ExternalTagBonus { get; set; } = [];
    public Dictionary<string, Anomaly> Anomalies { get; set; } = new();
    public Dictionary<string, Skill> Skills { get; set; } = new();
    
    public Affix RelatedElementDmg => Helpers.GetRelatedAffixDmg(Element);
    public Affix RelatedElementRes => Helpers.GetRelatedAffixRes(Element);
    
    public double Hp => Stats[Affix.Hp] * (1 + BonusStats[Affix.HpRatio]) + BonusStats[Affix.Hp];
    public double Atk => Stats[Affix.Atk] * (1 + BonusStats[Affix.AtkRatio]) + BonusStats[Affix.Atk];
    public double Def => Stats[Affix.Def] * (1 + BonusStats[Affix.DefRatio]) + BonusStats[Affix.Def];
    public double Pen => Stats[Affix.Pen] + BonusStats[Affix.Pen];
    public double PenRatio => Stats[Affix.PenRatio] + BonusStats[Affix.PenRatio];
    public double CritRate => Math.Min(Stats[Affix.CritRate] + BonusStats[Affix.CritRate], 1);
    public double CritDamage => Stats[Affix.CritDamage] + BonusStats[Affix.CritDamage];
    public double Impact => Stats[Affix.Impact] * (1 + BonusStats[Affix.ImpactRatio]) + BonusStats[Affix.Impact];
    public double AnomalyMastery => Stats[Affix.AnomalyMastery] * (1 + BonusStats[Affix.AnomalyMasteryRatio]) + BonusStats[Affix.AnomalyMastery];
    public double AnomalyProficiency => Stats[Affix.AnomalyProficiency] + BonusStats[Affix.AnomalyProficiency];
    public double EnergyRegen => Stats[Affix.EnergyRegen] * (1 + BonusStats[Affix.EnergyRegenRatio]) + BonusStats[Affix.EnergyRegen];
    public double ElementalDmgBonus => Stats[RelatedElementDmg] + BonusStats[RelatedElementDmg];
    public double ElementalResPen => Stats[RelatedElementRes] + BonusStats[RelatedElementRes];
    public double DmgBonus => Stats[Affix.DmgBonus] + BonusStats[Affix.DmgBonus];
    public double ResPen => Stats[Affix.ResPen] + BonusStats[Affix.ResPen];
    public double DazeBonus => Stats[Affix.DazeBonus] + BonusStats[Affix.DazeBonus];
    
    public SafeDictionary<Affix, double> CollectStats() => new() {
        [Affix.Hp] = Hp,
        [Affix.Atk] = Atk,
        [Affix.Def] = Def,
        [Affix.Pen] = Pen,
        [Affix.PenRatio] = PenRatio,
        [Affix.CritRate] = CritRate,
        [Affix.CritDamage] = CritDamage,
        [Affix.Impact] = Impact,
        [Affix.AnomalyMastery] = AnomalyMastery,
        [Affix.AnomalyProficiency] = AnomalyProficiency,
        [Affix.EnergyRegen] = EnergyRegen,
        [RelatedElementDmg] = ElementalDmgBonus,
        [RelatedElementRes] = ElementalResPen,
        [Affix.DmgBonus] = DmgBonus,
        [Affix.ResPen] = ResPen,
        [Affix.DazeBonus] = DazeBonus
    };

    /// <summary>
    /// Applies the agent's passive to the team
    /// </summary>
    /// <param name="team">Current team, including current agent</param>
    /// <returns>Collection of stats</returns>
    public virtual IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) => [];
    
    /// <summary>
    /// Applies agent's passive to themselves
    /// </summary>
    public virtual void ApplyPassive() { }
    
    /// <summary>
    /// Applies agent's ability's passive
    /// </summary>
    /// <param name="ability">Ability name</param>
    /// <returns><see cref="Stat"/> or null if ability has none</returns>
    public virtual Stat? ApplyAbilityPassive(string ability) => null;
}