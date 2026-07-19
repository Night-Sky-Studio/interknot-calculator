using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class PolarMetal : DriveDiscSet {
    public PolarMetal() : base(DriveDiscSetId.PolarMetal) {
        PartialBonus = [new(Affix.IceDmgBonus, 0.1)];
        FullBonus = [new(Affix.DmgBonus, 0.4, tags: [SkillTag.BasicAtk, SkillTag.Dash])];
    }
}
