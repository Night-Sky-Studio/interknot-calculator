using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FreedomBlues : DriveDiscSet {
    public FreedomBlues() : base(DriveDiscSetId.FreedomBlues) {
        PartialBonus = [new(Affix.AnomalyProficiency, 30)];
        FullBonus = [new(Affix.AnomalyBuildupBonus, 0.2, tags: [SkillTag.ExSpecial])];
    }
}
