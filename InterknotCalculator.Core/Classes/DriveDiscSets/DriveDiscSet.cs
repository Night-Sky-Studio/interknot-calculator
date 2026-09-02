using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public abstract class DriveDiscSet(uint id) {
    public ModifierKey Key { get; } = new("disc-set", id);
    
    public uint Id { get; } = id;

    public Stat[] PartialBonus { get; protected init; } = [];
    public Stat[] FullBonus { get; protected init; } = [];

    public virtual void ApplyPassive(Agent agent) { }
}