using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class PufferElectro : DriveDiscSet {
    public PufferElectro() : base(DriveDiscSetId.PufferElectro) {
        PartialBonus = [new(Affix.PenRatio, 0.08)];
        FullBonus = [
            new(Affix.DmgBonus, 0.2, tags: [SkillTag.Ultimate]),
            new(Affix.CombatAtkRatio, 0.15)
        ];
    }
}