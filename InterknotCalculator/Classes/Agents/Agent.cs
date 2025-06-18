using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

/// <summary>
/// Base Agent class
/// All agents should inherit from this class and be sealed
/// </summary>
public abstract class Agent(uint id) {
    private const double DamageTakenMultiplier = 1;
    public uint Id { get; } = id;
    public Speciality Speciality { get; set; }
    public Element Element { get; set; }
    public Rarity Rarity { get; set; }
    public Faction Faction { get; set; }
    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    public SafeDictionary<Affix, double> BonusStats { get; set; } = new();
    public SafeDictionary<Affix, double> ExternalBonus { get; set; } = new();
    public List<Stat> TagBonus { get; set; } = [];
    public List<Stat> ExternalTagBonus { get; set; } = [];
    public Dictionary<Element, Anomaly> Anomalies { get; set; } = new();
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

    /// <summary>
    /// Calculates standard damage for a given agent and skill
    /// </summary>
    /// <param name="skill">Skill name</param>
    /// <param name="scale">Skill level</param>
    /// <param name="enemy">Enemy instance</param>
    /// <returns><see cref="AgentAction"/> with calculated damage</returns>
    public AgentAction GetActionDamage(string skill, int scale, Enemy enemy) {
        var data = Skills[skill];
        var attribute = data.Scales[scale].Element ?? Element;
        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(attribute);
        var relatedAffixRes = Helpers.GetRelatedAffixRes(attribute);

        // Process all tag bonuses and apply if tag matches
        var tagDmgBonus = new SafeDictionary<Affix, double>();
        foreach (var stat in TagBonus) {
            if (stat.SkillTags.Contains(data.Tag)) {
                tagDmgBonus[stat.Affix] += stat.Value;
            }
        }

        // Apply ability passive if present
        var abilityPassive = ApplyAbilityPassive(skill);
        if (abilityPassive is { } passive) {
            tagDmgBonus.Add(passive.Affix, passive.Value);
        }

        // Process anomalies
        var buildup = GetAnomalyBuildup(skill, scale);
        enemy.AddAnomalyBuildup(this, buildup + buildup * BonusStats[Affix.AnomalyBuildupBonus]);
        
        // Calculate damage according to formula
        var baseDmgAttacker = data.Scales[scale].Damage / 100 * Atk;
        var dmgBonusMultiplier = 1 + ElementalDmgBonus + DmgBonus 
                                 + tagDmgBonus[relatedAffixDmg] + tagDmgBonus[Affix.DmgBonus]
                                 + data.Affixes[relatedAffixDmg] + data.Affixes[Affix.DmgBonus];
        var critMultiplier = 1 + Math.Min(CritRate + tagDmgBonus[Affix.CritRate] + data.Affixes[Affix.CritRate], 1)
            * (CritDamage + tagDmgBonus[Affix.CritDamage] + data.Affixes[Affix.CritDamage]);
        var resMultiplier = 1 + ElementalResPen + ResPen 
                            + tagDmgBonus[relatedAffixRes] + tagDmgBonus[Affix.ResPen]
                            + data.Affixes[relatedAffixRes] + data.Affixes[Affix.ResPen];

        var total = baseDmgAttacker * dmgBonusMultiplier * critMultiplier * enemy.GetDefenseMultiplier(this)
                    * resMultiplier * DamageTakenMultiplier * enemy.StunMultiplier;

        return new() {
            Name = $"{skill} { (scale == 0 && data.Scales.Count == 1 ? "" : scale + 1) }".Trim(),
            Tag = data.Tag,
            Damage = total
        };
    }

    public double GetAnomalyBuildup(string skill, int scale) {
        var data = Skills[skill];
        var baseBuildup = data.Scales[scale].AnomalyBuildup;
        if (baseBuildup == 0) return 0;

        var amBonus = AnomalyMastery / 100;
        var amBuildupBonus = 1 + BonusStats[Affix.AnomalyBuildupBonus] + data.Affixes[Affix.AnomalyBuildupBonus];
        var amBuildupRes = 1;

        return baseBuildup * amBonus * amBuildupBonus * amBuildupRes;
    }

    public virtual AgentAction GetAnomalyDamage(Element element, Enemy enemy) {
        // Agents can override default anomalies
        // ReSharper disable once InlineOutVariableDeclaration
        Anomaly data;
        if (!Anomalies.TryGetValue(element, out data!)) {
            data = Anomaly.GetAnomalyByElement(element);
        }
        
        // Some anomalies (Jane Doe - Assault) can crit
        double anomalyCritMultiplier = 1;

        if (data.CanCrit) {
            // These crit values are not affected by anything other than character's skill kit
            double anomalyCritRate = 0.05, anomalyCritDamage = 0.5;
            foreach (var bonus in data.Bonuses) {
                switch (bonus.Affix) {
                    case Affix.CritRate:
                        anomalyCritRate = Math.Min(bonus.Value, 1);
                        break;
                    case Affix.CritDamage:
                        anomalyCritDamage = bonus.Value;
                        break;
                }
            }

            anomalyCritMultiplier = 1 + anomalyCritRate * anomalyCritDamage;
        }

        // Calculate anomaly damage according to formula
        var anomalyBaseDmg = data.Scale / 100 * Atk;
        var anomalyProficiencyMultiplier = AnomalyProficiency / 100;
        const double anomalyLevelMultiplier = 2;
        var dmgBonusMultiplier = 1 + ElementalDmgBonus + DmgBonus;
        var resMultiplier = 1 + ElementalResPen + ResPen;

        var total = anomalyBaseDmg * anomalyProficiencyMultiplier * anomalyCritMultiplier * anomalyLevelMultiplier
                    * dmgBonusMultiplier * enemy.GetDefenseMultiplier(this) * resMultiplier;

        return new() {
            Name = data.ToString(),
            Tag = SkillTag.AttributeAnomaly,
            Damage = total
        };
    }
}