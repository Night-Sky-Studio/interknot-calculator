using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public abstract class Weapon(uint id) {
    public ModifierKey MainStatKey { get; } = new("weapon-main", id);
    public ModifierKey SecondaryStatKey { get; } = new("weapon-secondary", id);
    
    public uint Id { get; } = id;
    
    public Speciality Speciality { get; protected init; }
    public Rarity Rarity { get; protected init; }

    public Stat MainStat { get; protected init; }
    public Stat SecondaryStat { get; protected init; }
    public Dictionary<Affix, Modifier> Passive { get; protected init; } = new();
    public Dictionary<Affix, Modifier> ExternalBonus { get; protected init; } = new();

    public virtual void ApplyPassive(Agent agent) { }
    
    public virtual void RegisterHooks(Context ctx) { }
}