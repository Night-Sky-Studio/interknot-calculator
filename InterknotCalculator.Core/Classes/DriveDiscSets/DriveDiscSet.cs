using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public abstract class DriveDiscSet(uint id) {
    public uint Id { get; } = id;

    public Stat[] PartialBonus { get; init; } = [];
    public Stat[] FullBonus { get; init; } = [];

    public virtual void ApplyPassive(Agent agent) { }
}