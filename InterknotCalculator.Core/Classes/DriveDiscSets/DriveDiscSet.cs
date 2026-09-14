using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public abstract class DriveDiscSet(uint id) {
    public uint Id { get; } = id;

    public Stat[] PartialBonus { get; protected init; } = [];
    public Stat[] FullBonus { get; protected init; } = [];

    [Obsolete("Use RegisterHooks instead")]
    public virtual void ApplyPassive(Agent agent) { }

    public virtual void RegisterHooks(Context ctx, uint equipper = 0) { }
}