using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class BunnyInWonderland : DriveDiscSet {
    public BunnyInWonderland() : base(DriveDiscSetId.BunnyInWonderland) {
        PartialBonus = [new(Affix.HpRatio, 0.1)];
        FullBonus = [];
    }
}
