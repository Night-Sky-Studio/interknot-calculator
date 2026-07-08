using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class TheVault : Weapon {
    public TheVault() : base(WeaponId.TheVault) {
        Speciality = Speciality.Support;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.5);
        ExternalBonus = [new(Affix.DmgBonus, 0.24)];
        Passive = [new(Affix.EnergyRegen, 0.8)];
    }
}
