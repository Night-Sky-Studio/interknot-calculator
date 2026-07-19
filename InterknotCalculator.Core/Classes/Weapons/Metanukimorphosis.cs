using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class Metanukimorphosis : Weapon {
    public Metanukimorphosis() : base(WeaponId.Metanukimorphosis) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.6);
        ExternalBonus = [new(Affix.AnomalyProficiency, 60)];
        Passive = [new(Affix.AnomalyMastery, 30)];
    }
}
