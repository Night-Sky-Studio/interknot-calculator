using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Server;

public class CalcResult {
    public Dictionary<Affix, double> FinalStats { get; set; } = new();
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    public double Total { get; set; }
}