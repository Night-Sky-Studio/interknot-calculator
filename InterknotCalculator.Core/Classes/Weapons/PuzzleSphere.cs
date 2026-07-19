using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class PuzzleSphere : Weapon {
    public PuzzleSphere() : base(WeaponId.PuzzleSphere) {
        Speciality = Speciality.Rupture;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.AtkRatio, 0.25);
        Passive = [
            new(Affix.CritDamage, 0.256),
            new(Affix.DmgBonus, 0.32, tags: [SkillTag.ExSpecial])
        ];
    }
}
