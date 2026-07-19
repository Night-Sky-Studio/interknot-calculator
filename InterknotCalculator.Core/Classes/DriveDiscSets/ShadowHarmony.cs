using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ShadowHarmony : DriveDiscSet {
    public ShadowHarmony() : base(DriveDiscSetId.ShadowHarmony) {
        PartialBonus = [new(Affix.DmgBonus, 0.15, tags: [SkillTag.Aftershock, SkillTag.Dash])];
        FullBonus = [
            new(Affix.CombatAtkRatio, 0.12),
            new(Affix.CritRate, 0.12)
        ];
    }
}
