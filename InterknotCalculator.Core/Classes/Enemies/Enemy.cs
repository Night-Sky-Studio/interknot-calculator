using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Enemies;

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

    public Action<Enemy, Element, uint>? AttributeAnomalyTrigger { get; set; }

    public double GetDefenseMultiplier(double penRatio, double pen) => 
        LevelFactor / (Math.Max(Defense * (1 - penRatio) - pen, 0) + LevelFactor);
    
    private int AnomalyTriggerCount { get; set; } = 0;
    
    private bool WaitingForShatter { get; set; } = false;
    
    public Anomaly? AfflictedAnomaly { get; set; } = null;
    
    public void AddAnomalyBuildup(Agent agent, double value) {
        var element = agent.Element;
        var anomaly = Anomaly.GetAnomalyByElement(element);
        
        if (agent is ICustomAnomaly customAnomaly) {
            element = customAnomaly.AnomalyElement;
            anomaly = agent.Anomalies[element];
        }

        if (anomaly is not null) {
            anomaly.Stats = agent.FinalStats;
            anomaly.AgentId = agent.Id;
        }
        
        var buildup = AnomalyBuildup[element];
        buildup.AddContribution(agent.Id, value);
        
        var threshold = element == Element.Physical 
            ? AnomalyBuildupThreshold + AnomalyBuildupThreshold * 0.2
            : AnomalyBuildupThreshold;

        // Trigger Shatter manually
        if (WaitingForShatter) {
            buildup.AddContribution(agent.Id, threshold);
        }
        
        if (buildup.Current > threshold) {
            // Frostburn -> Prepare for Shatter
            // Freeze does not deal damage and can be ignored
            if (element is Element.Frost) {
                WaitingForShatter = !WaitingForShatter;

                if (WaitingForShatter == false) {
                    buildup.Reset();
                    AttributeAnomalyTrigger?.Invoke(this, Element.Ice, anomaly?.AgentId ?? 0); // Trigger Shatter
                    return;
                }
            }
            
            buildup.Reset();
            AnomalyBuildupThreshold = BaseAnomalyBuildupThreshold * Math.Pow(1.02, Math.Min(10, ++AnomalyTriggerCount));
            AttributeAnomalyTrigger?.Invoke(this, element, anomaly?.AgentId ?? 0);
            AfflictedAnomaly = anomaly;
        }
    }
}