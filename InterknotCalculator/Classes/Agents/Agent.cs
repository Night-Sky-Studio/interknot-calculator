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
    public SafeDictionary<Affix, double> ExternalBonus { get; set; } = new();
    public SafeDictionary<SkillTag, Stat> ExternalTagBonus { get; set; } = new();
    public Dictionary<string, Anomaly> Anomalies { get; set; } = new();
    public Dictionary<string, Skill> Skills { get; set; } = new();

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