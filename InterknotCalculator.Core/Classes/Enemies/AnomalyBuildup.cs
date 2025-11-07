namespace InterknotCalculator.Core.Classes.Enemies;

public readonly struct AnomalyBuildup {
    public AnomalyBuildup() { }
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

    public override string ToString() => $"Current: {Current}, Contributions: [{string.Join(", ", Contributions.Select(kv => $"{kv.Key}: {kv.Value}"))}]";
}