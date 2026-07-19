using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class ZanshinHerbCase : Weapon {
    public ZanshinHerbCase() : base(WeaponId.ZanshinHerbCase) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritDamage, 0.48);
        Passive = [
            new(Affix.CritRate, 0.2),
            new(Affix.ElectricDmgBonus, 0.4, tags: [SkillTag.Dash])
        ];
    }
}
