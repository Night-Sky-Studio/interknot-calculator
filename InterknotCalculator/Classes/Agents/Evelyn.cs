using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public class Evelyn : Agent {
    public Evelyn() : base(1321) {
        Speciality = Speciality.Attack;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.StarsOfLyra;

        Stats[Affix.Hp] = 7788;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 854 + 75;
        Stats[Affix.CritRate] = 0.05 + 0.144;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 93;
        Stats[Affix.AnomalyMastery] = 92;
        Stats[Affix.AnomalyProficiency] = 90;
        Stats[Affix.EnergyRegen] = 1.2;

        Skills["razor_wire"] = new(SkillTag.BasicAtk, [
            new(102.9, 38.8, Element: Element.Physical),
            new(124.8, 81.3, Element: Element.Physical),
            new(157.4, 94, Element: Element.Physical),
            new(373.7, 230, 139.03),
            new(447.8, 253, 152.91),
        ]);
        Skills["garrote"] = new(SkillTag.BasicAtk, [
            new(453, 182.7, 110.03),
            new(490.5, 187.5, 113.36)
        ]);

        Skills["piercing_ambush"] = new(SkillTag.Dash, [
            new(121, 45.7, Element: Element.Physical)
        ]);
        Skills["strangling_reversal"] = new(SkillTag.Counter, [
            new(421.4, 281.7, 70.03)
        ]);

        Skills["fierce_blade"] = new(SkillTag.QuickAssist, [
            new(155.2, 116.7, 70.03)
        ]);
        Skills["silent_protection"] = new(SkillTag.DefensiveAssist, [
            new(0, 407.7),
            new(0, 514.4),
            new(0, 250.4),
        ]);
        Skills["course_disruption"] = new(SkillTag.FollowUpAssist, [
            new(584.2, 374.4, 245.67)
        ]);

        Skills["locked_position"] = new(SkillTag.Special, [
            new(105.1, 78.7, 47.48)
        ]);
        Skills["binding_sunder_first"] = new(SkillTag.Special, [
            new(81.1, 61.3, 36.64),
            new(68.1, 51.6, 30.84)
        ]);
        Skills["binding_sunder_second"] = new(SkillTag.ExSpecial, [
            new(1082.4, 656, 444.62),
            new(120.1, 90.4, 56.2)
        ]);

        Skills["lunalux"] = new(SkillTag.Chain, [
            new(1658.7, 355.4, 290.17)
        ]);
        Skills["lunalux_garrote"] = new(SkillTag.Ultimate, [
            new(3977.3, 391.3, 236.7),
        ]);
    }

    public override void ApplyPassive() {
        BonusStats[Affix.CritRate] += 0.25;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Support)
            || team.Any(a => a.Speciality == Speciality.Stun)) {
            var bonusValue = 0.3;

            if (CritRate >= 0.8) {
                bonusValue *= 1.25;
            }

            return [new(bonusValue, Affix.DmgBonus, [SkillTag.Chain, SkillTag.Ultimate])];
        }

        return [];

    }
}