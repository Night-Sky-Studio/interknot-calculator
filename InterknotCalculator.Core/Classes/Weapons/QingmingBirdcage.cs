using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class QingmingBirdcage : Weapon {
    public QingmingBirdcage() : base(WeaponId.QingmingBirdcage) {
        Speciality = Speciality.Rupture;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 743);
        SecondaryStat = new(Affix.HpRatio, 0.3);
        Passive = [
            new(Affix.CritRate, 0.2),
            new(Affix.EtherDmgBonus, 0.16),
            new(Affix.EtherSheerBonus, 0.2, tags: [SkillTag.ExSpecial, SkillTag.Ultimate])
        ];
    }
}
