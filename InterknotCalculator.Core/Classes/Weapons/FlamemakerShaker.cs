using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class FlamemakerShaker : Weapon {
    public FlamemakerShaker() : base(WeaponId.FlamemakerShaker) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.24);
        Passive = [
            new(Affix.DmgBonus, 0.35, tags: [SkillTag.ExSpecial, SkillTag.QuickAssist, SkillTag.DefensiveAssist, SkillTag.EvasiveAssist, SkillTag.FollowUpAssist]),
            new(Affix.AnomalyProficiency, 50)
        ];
    }
}
