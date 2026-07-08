using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class YunkuiTales : DriveDiscSet {
    public YunkuiTales() : base(DriveDiscSetId.YunkuiTales) {
        PartialBonus = [new(Affix.HpRatio, 0.1)];
        FullBonus = [
            new(Affix.CritRate, 0.12),
            new(Affix.SheerBonus, 0.1)
        ];
    }
}
