using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class SpectralGaze : Weapon {
    public SpectralGaze() : base(WeaponId.SpectralGaze) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.ElectricResPen, 0.25),
            new(Affix.ImpactRatio, 0.2)
        ];
    }
}
