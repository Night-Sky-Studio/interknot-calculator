using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class CauldronOfClarity : Weapon {
    public CauldronOfClarity() : base(WeaponId.CauldronOfClarity) {
        Speciality = Speciality.Rupture;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.HpRatio, 0.25);
        Passive = [
            new(Affix.DmgBonus, 0.192),
            new(Affix.CritRate, 0.104)
        ];
    }
}
