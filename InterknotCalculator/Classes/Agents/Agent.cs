using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public abstract class Agent {
    public Speciality Speciality { get; set; }
    public Element Element { get; set; }
    public Rarity Rarity { get; set; }
    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    public Dictionary<string, Anomaly> Anomalies { get; set; } = new();
    public Dictionary<string, Skill> Skills { get; set; } = new();

    public virtual void ApplyPassive() { }
}