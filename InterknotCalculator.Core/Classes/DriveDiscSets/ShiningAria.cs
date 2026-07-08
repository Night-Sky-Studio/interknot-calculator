using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ShiningAria : DriveDiscSet {
    public ShiningAria() : base(DriveDiscSetId.ShiningAria) {
        PartialBonus = [new(Affix.EtherDmgBonus, 0.1)];
        FullBonus = [
            new(Affix.AnomalyProficiency, 36),
            new(Affix.DmgBonus, 0.25)
        ];
    }
}
