using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Server;

public class CalcResult {
    public FinalStats FinalStats { get; set; } = new();
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    public double Total { get; set; }
}