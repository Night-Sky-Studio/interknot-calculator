using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class SwingJazz : DriveDiscSet {
    public SwingJazz() : base(DriveDiscSetId.SwingJazz) {
        PartialBonus = [new(Affix.EnergyRegenRatio, 0.2)];
        FullBonus = [new(Affix.DmgBonus, 0.15, tags: [SkillTag.Chain, SkillTag.Ultimate])];
    }
}
