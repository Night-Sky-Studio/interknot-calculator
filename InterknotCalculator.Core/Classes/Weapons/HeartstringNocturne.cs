using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class HeartstringNocturne : Weapon {
    public HeartstringNocturne() : base(WeaponId.HeartstringNocturne) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.CritDamage, 0.5),
            new(Affix.FireResPen, 0.25, tags: [SkillTag.Chain, SkillTag.Ultimate])
        ];
    }
}
