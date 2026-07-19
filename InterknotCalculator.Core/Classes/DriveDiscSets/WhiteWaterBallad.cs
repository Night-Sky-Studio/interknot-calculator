using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class WhiteWaterBallad : DriveDiscSet {
    public WhiteWaterBallad() : base(DriveDiscSetId.WhiteWaterBallad) {
        PartialBonus = [new(Affix.PhysicalDmgBonus, 0.1)];
        FullBonus = [
            new(Affix.CritRate, 0.2),
            new(Affix.CombatAtkRatio, 0.1)
        ];
    }
}
