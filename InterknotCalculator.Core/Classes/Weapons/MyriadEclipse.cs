using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class MyriadEclipse : Weapon {
    public MyriadEclipse() : base(WeaponId.MyriadEclipse) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.CritDamage, 0.45),
            new(Affix.ResPen, 0.25, tags: [SkillTag.ExSpecial, SkillTag.Chain, SkillTag.Ultimate])
        ];
    }
}
