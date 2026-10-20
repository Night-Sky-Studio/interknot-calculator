using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Events;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Enemies;

public abstract class Enemy(double defense, double levelFactor, double anomalyBuildupThreshold) {
    public MutableStat Defense { get; } = new(defense);
    public double LevelFactor { get; } = levelFactor;
    private double BaseAnomalyBuildupThreshold { get; } = anomalyBuildupThreshold;
    public double AnomalyBuildupThreshold { get; private set; } = anomalyBuildupThreshold;
    public Progress Daze { get; set; }
    public MutableStat StunMultiplier { get; set; } = new(1);
    // TODO(IKC-53): Decouple Enemy stun state from Stun Multiplier
    public bool IsStunned => StunMultiplier.BaseValue > 1;

    public Dictionary<Element, AnomalyBuildup> AnomalyBuildup { get; } = new();
    private AnomalyBuildup GetBuildup(Element el) => 
        AnomalyBuildup.TryGetValue(el, out var buildup) ? buildup : AnomalyBuildup[el] = new();
    
    /// <summary>
    /// Enemy DEF multiplier of a single hit
    /// </summary>
    /// <remarks>See "DEF Modifier" in IKC-A-2</remarks>
    /// <param name="defPen">Attacker's DEF ignore for this hit, as a positive fraction</param>
    /// <param name="penRatio">Attacker's PEN Ratio</param>
    /// <param name="pen">Attacker's flat PEN</param>
    public double GetDefenseMultiplier(double defPen, double penRatio, double pen) {
        // Target DEF = Base DEF * (1 + DEF% - DEF Reduction% - DEF Ignore%) + Flat DEF
        // "DEF reduction" is untagged CombatRatio modifier
        // "DEF ignore" belongs to the attacker
        var targetDef = Defense.Value - Defense.BaseValue * defPen;
        var effectiveDef = Math.Max(targetDef * (1 - penRatio) - pen, 0);
        return LevelFactor / (effectiveDef + LevelFactor);
    }
    
    private int AnomalyTriggerCount { get; set; }
    
    private bool WaitingForShatter { get; set; }
    
    public Anomaly? AfflictedAnomaly { get; set; }
    
    private void TryTriggerAnomaly(Context ctx, Agent agent, Element element) {
        var buildup = GetBuildup(element);
        
        var threshold = element.Matches(Element.Physical) 
            ? AnomalyBuildupThreshold * 1.2
            : AnomalyBuildupThreshold;

        // Trigger Shatter manually
        if (WaitingForShatter) {
            buildup.AddContribution(agent.Id, threshold);
        }
        
        if (buildup.Current > threshold) {
            var anomalyThresholdReachedArgs = new AnomalyThresholdEventArgs(agent, element);
            ctx.Events.AnomalyThreshold(ctx, anomalyThresholdReachedArgs);
            
            if (anomalyThresholdReachedArgs.Ignore) return;
            
            // Frostburn -> Prepare for Shatter
            // Freeze does not deal damage and can be ignored
            if (element is Element.Frost) {
                WaitingForShatter = !WaitingForShatter;

                // ReSharper disable once RedundantBoolCompare
                if (WaitingForShatter == false) {
                    buildup.Reset();
                    ctx.Events.AnomalyTriggered(ctx, new(agent, Element.Ice)); // Trigger Shatter
                    return;
                }
            }
            var stats = buildup.Snapshots[agent.Id];
            buildup.Reset();
            AnomalyBuildupThreshold = BaseAnomalyBuildupThreshold * Math.Pow(1.02, Math.Min(10, ++AnomalyTriggerCount));
            ctx.Events.AnomalyTriggered(ctx, new(agent, element));
            
            Anomaly? anomaly = null;
            if (agent is ICustomAnomaly && agent.Anomalies.TryGetValue(element, out var customAnomaly)) {
                // TODO: log a warning if agent is ICustomAnomaly,
                //       but doesn't have an anomaly for the given element
                anomaly = customAnomaly;
            }
            anomaly ??= Anomaly.GetDefaultAnomalyByElement(element);
            
            AfflictedAnomaly = anomaly with { Stats = stats, AgentId = agent.Id };
        }
    }
    
    public void AddBuildupContribution(Context ctx, Agent agent, double value, Element element) {
        var buildup = GetBuildup(element);
        if (!buildup.Snapshots.ContainsKey(agent.Id))
            buildup.Snapshots[agent.Id] = agent.CollectStats();
        buildup.AddContribution(agent.Id, value);
        ctx.Events.AnomalyBuildup(ctx, new(agent, buildup.Current, element));
    }
    
    public void AddAnomalyBuildup(Context ctx, Agent agent, double value, Element? elementOverride = null) {
        Element element;
        if (elementOverride is { } overrideElement) {
            element = overrideElement;
        } else if (agent is ICustomAnomaly customAnomaly) {
            element = customAnomaly.AnomalyElement;
        } else {
            element = agent.Element;
        }

        AddBuildupContribution(ctx, agent, value, element);
        TryTriggerAnomaly(ctx, agent, element);
    }
}