using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class TheSkyAblaze : DriveDiscSet {
    public TheSkyAblaze() : base(DriveDiscSetId.TheSkyAblaze) {
        PartialBonus = [new(Affix.EtherDmgBonus, 0.1)];
        FullBonus = [];
    }
}
