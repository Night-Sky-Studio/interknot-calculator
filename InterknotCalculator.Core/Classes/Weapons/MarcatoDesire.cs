using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class MarcatoDesire : Weapon {
    public MarcatoDesire() : base(WeaponId.MarcatoDesire) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.CritRate, 0.2);
        Passive = [new(Affix.CombatAtkRatio, 0.192)];
    }
}
