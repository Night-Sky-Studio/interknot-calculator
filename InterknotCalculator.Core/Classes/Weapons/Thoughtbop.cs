using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class Thoughtbop : Weapon {
    public Thoughtbop() : base(WeaponId.Thoughtbop) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.6);
        ExternalBonus = [new(Affix.DmgBonus, 0.125 * 2), new(Affix.CombatAtkRatio, 0.1)];
    }
}