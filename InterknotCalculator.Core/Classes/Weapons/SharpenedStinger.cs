using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class SharpenedStinger : Weapon {
    public SharpenedStinger() : base(WeaponId.SharpenedStinger) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AnomalyProficiency, 90);
        Passive = [
            new(Affix.PhysicalDmgBonus, 0.36),
            new(Affix.AnomalyBuildupBonus, 0.4)
        ];
    }
}
