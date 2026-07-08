using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class StarlightEngine : Weapon {
    public StarlightEngine() : base(WeaponId.StarlightEngine) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.AtkRatio, 0.25);
        Passive = [new(Affix.CombatAtkRatio, 0.192)];
    }
}