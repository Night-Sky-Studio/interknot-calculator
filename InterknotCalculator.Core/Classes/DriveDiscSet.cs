using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Classes;

public record DriveDiscSet(IEnumerable<Stat> PartialBonus, IEnumerable<Stat> FullBonus) {
    [JsonIgnore]
    public Action<Agent>? ApplyPassive { get; set; } = null;
}