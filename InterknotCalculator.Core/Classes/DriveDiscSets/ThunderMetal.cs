using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ThunderMetal : DriveDiscSet {
    public ThunderMetal() : base(DriveDiscSetId.ThunderMetal) {
        PartialBonus = [new(Affix.ElectricDmgBonus, 0.1)];
        FullBonus = [new(Affix.CombatAtkRatio, 0.28)];
    }
}
