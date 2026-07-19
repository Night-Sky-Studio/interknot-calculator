using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class BashfulDemon : Weapon {
    public BashfulDemon() : base(WeaponId.BashfulDemon) {
        Speciality = Speciality.Support;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.AtkRatio, 0.25);
        ExternalBonus = [new(Affix.CombatAtkRatio, 0.128)];
        Passive = [new(Affix.IceDmgBonus, 0.24)];
    }
}
