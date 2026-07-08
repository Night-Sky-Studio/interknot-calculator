using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class HailstormShrine : Weapon {
    public HailstormShrine() : base(WeaponId.HailstormShrine) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 743);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.CritDamage, 0.5),
            new(Affix.IceDmgBonus, 0.4)
        ];
    }
}
