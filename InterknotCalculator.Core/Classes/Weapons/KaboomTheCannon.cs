using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class KaboomTheCannon : Weapon {
    public KaboomTheCannon() : base(WeaponId.KaboomTheCannon) {
        Speciality = Speciality.Support;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.5);
        ExternalBonus = [new(Affix.CombatAtkRatio, 0.16)];
    }
}
