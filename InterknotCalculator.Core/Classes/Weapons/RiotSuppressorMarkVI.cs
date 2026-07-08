using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class RiotSuppressorMarkVI : Weapon {
    public RiotSuppressorMarkVI() : base(WeaponId.RiotSuppressorMarkVI) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritDamage, 0.48);
        Passive = [
            new(Affix.CritRate, 0.15),
            new(Affix.DmgBonus, 0.35, tags: [SkillTag.Special, SkillTag.ExSpecial])
        ];
    }
}
