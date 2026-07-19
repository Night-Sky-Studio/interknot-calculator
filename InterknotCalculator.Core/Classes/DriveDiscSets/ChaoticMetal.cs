using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ChaoticMetal : DriveDiscSet {
    public ChaoticMetal() : base(DriveDiscSetId.ChaoticMetal) {
        PartialBonus = [new(Affix.EtherDmgBonus, 0.1)];
        FullBonus = [
            new(Affix.CritDamage, 0.2),
            new(Affix.CritDamage, 0.33)
        ];
    }
}
