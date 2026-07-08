using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class FlightOfFancy : Weapon {
    public FlightOfFancy() : base(WeaponId.FlightOfFancy) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AnomalyProficiency, 90);
        Passive = [new(Affix.AnomalyBuildupBonus, 0.4)];
    }
}
