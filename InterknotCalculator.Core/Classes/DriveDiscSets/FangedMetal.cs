using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FangedMetal : DriveDiscSet {
    public FangedMetal() : base(DriveDiscSetId.FangedMetal) {
        PartialBonus = [new(Affix.PhysicalDmgBonus, 0.1)];
        FullBonus = [new(Affix.DmgBonus, 0.35)];
    }
}
