using System.Collections;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Enums;

// Anomaly buildup is a progress bar from 0 to N
// It indicates the amount of anomaly that has been applied to the enemy
// Every single agent can apply an anomaly of their own element
// Then, when the bar fills up, the attribute anomaly triggers and deals damage to the enemy
// This damage is based on the total anomaly applied to the enemy by each agent
// (ex. if  Evelyn applies 25% of Fire (Burn) anomaly and Burnice applies remaining 75% of same anomaly,
// then the Burn damage will be calculated from the percentage of applied anomaly of the agent that
// triggered the attribute anomaly)
// Different types of anomalies can trigger Disorder which happens when already accumulated anomaly is being
// replaced and triggered by the new anomaly type.
// (ex. Enemy has 50% of Fire (Burn) anomaly and Yanagi applies 100% of Electric (Shock) anomaly,
// the Disorder will be triggered, anomaly buildup will be reset to 0 and the enemy will take
// damage based on the Disorder damage formula)

namespace InterknotCalculator.Classes.Enemies;

public record AnomalyBuildup {
    public double Current => Contributions.Values.Sum();
    public Dictionary<uint, double> Contributions { get; } = new();
    
    public void AddContribution(uint agentId, double contribution) {
        if (!Contributions.TryAdd(agentId, contribution)) {
            Contributions[agentId] += contribution;
        }
    }
    
    public void Reset() {
        Contributions.Clear();
    }

    // percentage of total contributions
    public double GetContribution(uint agentId) {
        if (!Contributions.TryGetValue(agentId, out var contribution)) {
            return 0;
        }
        
        return contribution / Current * 100;
    }
}