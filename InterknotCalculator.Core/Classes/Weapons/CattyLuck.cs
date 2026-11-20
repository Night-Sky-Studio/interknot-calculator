using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class CattyLuck : Weapon {
    public CattyLuck() : base(WeaponId.CattyLuck) {
        Speciality = Speciality.Armorer;
        Rarity = Rarity.A;
        MainStat = new(Affix.Def, 356);
        SecondaryStat = new(Affix.DefRatio, 0.4);
        Passive = [
            new(Affix.DefRatio, 0.12 * 2)
        ];
    }
}