using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ThunderMetal : DriveDiscSet {
    public ThunderMetal() : base(DriveDiscSetId.ThunderMetal) {
        PartialBonus = [new(Affix.ElectricDmgBonus, 0.1)];
        // TODO(IKC-13): Needs context to track Shock anomaly on an enemy
        FullBonus = [new(Affix.CombatAtkRatio, 0.28)];
    }
}
