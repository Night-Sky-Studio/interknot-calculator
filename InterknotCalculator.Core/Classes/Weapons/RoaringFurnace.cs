using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class RoaringFurnace : Weapon {
    public RoaringFurnace() : base(WeaponId.RoaringFurnace) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        ExternalBonus = [new(Affix.DmgBonus, 0.2)];
        Passive = [new(Affix.DazeBonus, 0.28, tags: [SkillTag.ExSpecial, SkillTag.Chain, SkillTag.Ultimate])];
    }
}
