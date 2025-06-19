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