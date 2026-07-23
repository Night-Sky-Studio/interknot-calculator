using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class YesterdayCalls : Weapon {
    public YesterdayCalls() : base(WeaponId.YesterdayCalls) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [new(Affix.DazeBonus, 0.27)];
        ExternalBonus = [new(Affix.CritDamage, 0.3)];
    }
}