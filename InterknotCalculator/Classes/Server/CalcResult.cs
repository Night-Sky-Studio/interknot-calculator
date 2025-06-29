using System.Text.Json.Serialization;
using InterknotCalculator.Classes.Enemies;
namespace InterknotCalculator.Classes.Server;

public class CalcResult {
    public FinalStats FinalStats { get; set; } = new();
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    [JsonIgnore]
    public Enemy? Enemy { get; set; }
    public double Total { get; set; }
}