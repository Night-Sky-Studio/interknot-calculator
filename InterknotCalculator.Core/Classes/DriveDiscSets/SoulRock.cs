using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class SoulRock : DriveDiscSet {
    public SoulRock() : base(DriveDiscSetId.SoulRock) {
        PartialBonus = [new(Affix.DefRatio, 0.16)];
        FullBonus = [];
    }
}
