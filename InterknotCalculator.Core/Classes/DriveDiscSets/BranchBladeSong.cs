using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class BranchBladeSong : DriveDiscSet {
    public BranchBladeSong() : base(DriveDiscSetId.BranchBladeSong) {
        PartialBonus = [new(Affix.CritDamage, 0.16)];
        FullBonus = [
            new(Affix.CritDamage, 0.3),
            new(Affix.CritRate, 0.12)
        ];
    }
}
