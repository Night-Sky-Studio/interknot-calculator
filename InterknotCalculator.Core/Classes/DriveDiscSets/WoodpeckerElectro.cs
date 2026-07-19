using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class WoodpeckerElectro : DriveDiscSet {
    public WoodpeckerElectro() : base(DriveDiscSetId.WoodpeckerElectro) {
        PartialBonus = [new(Affix.CritRate, 0.08)];
        FullBonus = [new(Affix.CombatAtkRatio, 0.27)];
    }
}