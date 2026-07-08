using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ChaosJazz : DriveDiscSet {
    public ChaosJazz() : base(DriveDiscSetId.ChaosJazz) {
        PartialBonus = [new(Affix.AnomalyProficiency, 30)];
        FullBonus = [
            new(Affix.FireDmgBonus, 0.15),
            new(Affix.ElectricDmgBonus, 0.15),
            new(Affix.DmgBonus, 0.2, tags: [SkillTag.ExSpecial, SkillTag.QuickAssist, SkillTag.DefensiveAssist, SkillTag.EvasiveAssist, SkillTag.FollowUpAssist])
        ];
    }
}
