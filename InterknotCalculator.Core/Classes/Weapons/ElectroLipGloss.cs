using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class ElectroLipGloss : Weapon {
    public ElectroLipGloss() : base(WeaponId.ElectroLipGloss) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.AnomalyProficiency, 75);
        Passive = [
            new(Affix.CombatAtkRatio, 0.16),
            new(Affix.DmgBonus, 0.25)
        ];
    }
}
