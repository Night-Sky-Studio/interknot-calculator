using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class SeveredInnocence : Weapon {
    public SeveredInnocence() : base(WeaponId.SeveredInnocence) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritDamage, 0.48);
        Passive = [
            new(Affix.CritDamage, 0.6),
            new(Affix.ElectricDmgBonus, 0.2)
        ];
    }
}
