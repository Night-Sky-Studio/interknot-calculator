using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class Timeweaver : Weapon {
    public Timeweaver() : base(WeaponId.Timeweaver) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        Passive = [
            new(Affix.AnomalyBuildupBonus, 0.3),
            new(Affix.AnomalyProficiency, 75)
        ];
    }
}
