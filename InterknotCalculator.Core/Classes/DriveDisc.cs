using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public record DriveDisc(
    uint SetId, 
    uint Slot, 
    Rarity Rarity, 
    Stat MainStat, 
    IEnumerable<Stat> SubStats
);