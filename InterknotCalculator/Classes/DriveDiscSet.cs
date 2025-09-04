using System.Text.Json.Serialization;
using InterknotCalculator.Classes.Agents;

namespace InterknotCalculator.Classes;

public record DriveDiscSet(IEnumerable<Stat> PartialBonus, IEnumerable<Stat> FullBonus) {
    [JsonIgnore]
    public Action<Agent>? ApplyPassive { get; set; } = null;
}