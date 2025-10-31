using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public record DriveDisc(
    uint SetId, 
    uint Slot, 
    Rarity Rarity, 
    Stat MainStat, 
    IEnumerable<Stat> SubStats
);