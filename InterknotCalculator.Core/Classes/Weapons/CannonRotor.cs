using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class CannonRotor : Weapon {
    public CannonRotor() : base(WeaponId.CannonRotor) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.CritRate, 0.2);
        Passive = [new(Affix.AtkRatio, 0.12)];
    }
}
