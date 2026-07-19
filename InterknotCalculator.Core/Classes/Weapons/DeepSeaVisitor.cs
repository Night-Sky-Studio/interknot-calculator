using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class DeepSeaVisitor : Weapon {
    public DeepSeaVisitor() : base(WeaponId.DeepSeaVisitor) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.IceDmgBonus, 0.25),
            new(Affix.CritRate, 0.2)
        ];
    }
}
