using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public record Weapon {
    public Speciality Speciality { get; set; }
    public Rarity Rarity { get; set; }

    public required Stat MainStat { get; set; }
    public required Stat SecondaryStat { get; set; }
    public IEnumerable<Stat> Passive { get; set; } = [];
    public IEnumerable<Stat> ExternalBonus { get; set; } = [];
}