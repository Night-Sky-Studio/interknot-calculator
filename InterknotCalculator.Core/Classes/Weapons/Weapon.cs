using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public abstract class Weapon(uint id) {
    public uint Id { get; } = id;
    
    public Speciality Speciality { get; init; }
    public Rarity Rarity { get; init; }

    public Stat MainStat { get; init; }
    public Stat SecondaryStat { get; init; }
    public Stat[] Passive { get; init; } = [];
    public Stat[] ExternalBonus { get; init; } = [];

    public virtual void ApplyPassive(Agent agent) { }
}