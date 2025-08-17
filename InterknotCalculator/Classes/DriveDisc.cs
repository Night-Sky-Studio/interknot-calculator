using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;


public record DriveDisc(uint SetId, uint Slot, Rarity Rarity, Stat MainStat, Stat[] SubStats) {
    public override string ToString() => 
        $"DriveDisc[{Slot}]({SetId}, {Rarity}, {MainStat}, [{string.Join(", ", SubStats)}])";
}