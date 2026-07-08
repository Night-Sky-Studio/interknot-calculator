using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class PracticedPerfection : Weapon {
    public PracticedPerfection() : base(WeaponId.PracticedPerfection) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        Passive = [
            new(Affix.AnomalyMastery, 60),
            new(Affix.PhysicalDmgBonus, 0.4)
        ];
    }
}
