using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

using DiscStat = (uint Level, Stat Stat);

public record DriveDisc(uint SetId, uint Slot, Rarity Rarity, Stat MainStat, IEnumerable<DiscStat> SubStats);