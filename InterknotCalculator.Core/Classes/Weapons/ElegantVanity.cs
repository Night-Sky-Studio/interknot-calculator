using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class ElegantVanity : Weapon {
    public ElegantVanity() : base(WeaponId.ElegantVanity) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        ExternalBonus = [new(Affix.DmgBonus, 0.2)];
    }
}
