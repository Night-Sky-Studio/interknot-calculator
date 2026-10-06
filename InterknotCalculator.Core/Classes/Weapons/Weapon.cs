using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public abstract class Weapon(uint id) {
    public uint Id { get; } = id;
    
    public Speciality Speciality { get; protected init; }
    public Rarity Rarity { get; protected init; }

    public Stat MainStat { get; protected init; }
    public Stat SecondaryStat { get; protected init; }
    public List<Stat> Passive { get; protected init; } = new();

    [Obsolete("Use RegisterHooks instead")]
    public virtual void ApplyPassive(Agent agent) { }
    
    public virtual void RegisterHooks(Context ctx, uint equipper = 0) { }
}