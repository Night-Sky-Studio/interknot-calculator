using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class HormonePunk : DriveDiscSet {
    public HormonePunk() : base(DriveDiscSetId.HormonePunk) {
        PartialBonus = [new(Affix.AtkRatio, 0.1)];
        FullBonus = [new(Affix.CombatAtkRatio, 0.25)];
    }
}
