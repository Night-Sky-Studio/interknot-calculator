using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class TremorTrigramVessel : Weapon {
    public TremorTrigramVessel() : base(WeaponId.TremorTrigramVessel) {
        Speciality = Speciality.Defense;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.AtkRatio, 0.5);
        ExternalBonus = [new(Affix.DmgBonus, 0.4, tags: [SkillTag.ExSpecial, SkillTag.Ultimate])];
    }
}
