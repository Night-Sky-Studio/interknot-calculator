namespace InterknotCalculator.Classes.Server;

public class CalcResult {
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    public double Total { get; set; }
}