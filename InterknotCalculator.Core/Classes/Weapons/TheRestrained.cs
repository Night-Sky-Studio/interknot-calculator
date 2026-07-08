using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class TheRestrained : Weapon {
    public TheRestrained() : base(WeaponId.TheRestrained) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 684);
        SecondaryStat = new(Affix.ImpactRatio, 0.18);
        Passive = [
            new(Affix.CombatAtkRatio, 0.3, tags: [SkillTag.BasicAtk]),
            new(Affix.DazeBonus, 0.3, tags: [SkillTag.BasicAtk])
        ];
    }
}
