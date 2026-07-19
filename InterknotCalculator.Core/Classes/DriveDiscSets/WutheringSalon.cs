using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class WutheringSalon : DriveDiscSet {
    public WutheringSalon() : base(DriveDiscSetId.WutheringSalon) {
        PartialBonus = [new(Affix.WindDmgBonus, 0.1)];
        FullBonus = [
            new(Affix.AnomalyProficiency, 50),
            new(Affix.DmgBonus, 0.18)
        ];
    }
}
