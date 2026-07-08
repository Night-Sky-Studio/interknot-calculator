using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class TheBrimstone : Weapon {
    public TheBrimstone() : base(WeaponId.TheBrimstone) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 684);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        Passive = [new(Affix.CombatAtkRatio, 0.28)];
    }
}
