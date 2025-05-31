using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Enemies;

public abstract class Enemy(double defense, double levelFactor, double anomalyBuildupThreshold) {
    public double Defense { get; } = defense;
    public double LevelFactor { get; } = levelFactor;
    private double BaseAnomalyBuildupThreshold { get; } = anomalyBuildupThreshold;
    public double AnomalyBuildupThreshold { get; private set; } = anomalyBuildupThreshold;
    public Progress Daze { get; set; }
    public double StunMultiplier { get; set; } = 1.5;

    public SafeDictionary<Affix, double> Stats { get; set; } = new();

    public Dictionary<Element, AnomalyBuildup> AnomalyBuildup { get; } = new() {
        [Element.Ice] = new(),
        [Element.Frost] = new(),
        [Element.Fire] = new(),
        [Element.Electric] = new(),
        [Element.Ether] = new(),
        [Element.AuricInk] = new(),
        [Element.Physical] = new()
    };

    public Action<Element>? AttributeAnomalyTrigger { get; set; }

    public double GetDefenseMultiplier(Agent agent) => 
        LevelFactor / (Math.Max(Defense * (1 - agent.PenRatio) - agent.Pen, 0) + LevelFactor);
    
    private int AnomalyTriggerCount { get; set; } = 0;
    
    public void AddAnomalyBuildup(Agent agent, double value) {
        var buildup = AnomalyBuildup[agent.Element];
        buildup.AddContribution(agent.Id, value);
        
        if (buildup.Current > AnomalyBuildupThreshold) {
            buildup.Reset();
            AnomalyBuildupThreshold = BaseAnomalyBuildupThreshold * Math.Pow(1.02, AnomalyTriggerCount++);
            AttributeAnomalyTrigger?.Invoke(agent.Element);
        }
    }
}