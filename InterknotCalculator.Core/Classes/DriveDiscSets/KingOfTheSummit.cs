using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class KingOfTheSummit : DriveDiscSet {
    public KingOfTheSummit() : base(DriveDiscSetId.KingOfTheSummit) {
        PartialBonus = [new(Affix.DazeBonus, 0.06)];
        FullBonus = [new(Affix.CritDamage, 0.3)];
    }
}
