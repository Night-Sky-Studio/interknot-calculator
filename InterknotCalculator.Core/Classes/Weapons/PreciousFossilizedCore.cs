using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class PreciousFossilizedCore : Weapon {
    public PreciousFossilizedCore() : base(WeaponId.PreciousFossilizedCore) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.ImpactRatio, 0.15);
        Passive = [new(Affix.DazeBonus, 0.32)];
    }
}