using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public record DriveDisc(
    uint SetId,
    uint Slot,
    Rarity Rarity,
    Stat MainStat,
    IEnumerable<Stat> SubStats
) {
    public ModifierKey Key { get; } = new($"disc-{Rarity}-{Slot}", SetId);
    public override string ToString() => $"Disc({SetId}, {Slot}, {Rarity}, {MainStat}, [{string.Join(", ", SubStats)}])";
}