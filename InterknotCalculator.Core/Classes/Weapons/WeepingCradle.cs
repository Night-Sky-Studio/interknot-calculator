using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class WeepingCradle : Weapon {
    public WeepingCradle() : base(WeaponId.WeepingCradle) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 684);
        SecondaryStat = new(Affix.PenRatio, 0.24);
        ExternalBonus = [new(Affix.DmgBonus, 0.202)];
    }
}
