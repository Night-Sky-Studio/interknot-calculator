using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class HellfireGears : Weapon {
    public HellfireGears() : base(WeaponId.HellfireGears) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 684);
        SecondaryStat = new(Affix.ImpactRatio, 0.18);
        Passive = [new(Affix.ImpactRatio, 0.2)];
    }
}
