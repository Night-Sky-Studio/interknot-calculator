using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Enemies;

namespace InterknotCalculator.Core.Classes.Server;

public record CalcResult {
    public FinalStats FinalStats { get; set; } = new();
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    [JsonIgnore]
    public Enemy? Enemy { get; set; }
    public double Total { get; set; }
}