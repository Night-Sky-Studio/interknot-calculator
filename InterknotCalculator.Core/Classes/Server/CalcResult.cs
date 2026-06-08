using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Enemies;

namespace InterknotCalculator.Core.Classes.Server;

public record CalcResult {
    public FinalStats FinalStats { get; set; } = new();
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    public Enemy Enemy { get; set; } = new NotoriousDullahan();
    public double Total { get; set; }
}