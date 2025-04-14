using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public class Evelyn : Agent {
    public Evelyn() {
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

        Skills["razor_wire"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(102.9, 38.8, Element.Physical),
                new(124.8, 81.3, Element.Physical),
                new(157.4, 94, Element.Physical),
                new(373.7, 230),
                new(447.8, 253),
            ]
        };
        Skills["garrote"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(453, 182.7),
                new(490.5, 187.5)
            ]
        };

        Skills["piercing_ambush"] = new() {
            Tag = SkillTag.Dash,
            Scales = [
                new(121, 45.7, Element.Physical)
            ]
        };
        Skills["strangling_reversal"] = new () {
            Tag = SkillTag.Counter,
            Scales = [
                new(421.4, 281.7)
            ]
        };

        Skills["fierce_blade"] = new() {
            Tag = SkillTag.QuickAssist,
            Scales = [new(155.2, 116.7)]
        };
        Skills["silent_protection"] = new() {
            Tag = SkillTag.DefensiveAssist,
            Scales = [
                new(0, 407.7),
                new(0, 514.4),
                new(0, 250.4),
            ]
        };
        Skills["course_disruption"] = new() {
            Tag = SkillTag.FollowUpAssist,
            Scales = [
                new(584.2, 374.4)            
            ]
        };

        Skills["locked_position"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new(105.1, 78.7)
            ]
        };
        Skills["binding_sunder_first"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new(81.1, 61.3),
                new(68.1, 51.6)
            ]
        };
        Skills["binding_sunder_second"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(1082.4, 656),
                new(120.1, 90.4)
            ]
        };

        Skills["lunalux"] = new() {
            Tag = SkillTag.Chain,
            Scales = [
                new(1658.7, 355.4)
            ]
        };
        Skills["lunalux_garrote"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [
                new(3977.3, 391.3)
            ]
        };
    }

    public override void ApplyPassive() {
        Stats[Affix.CritRate] += 0.25;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.All(a => a.Speciality != Speciality.Support) && 
            team.All(a => a.Speciality != Speciality.Stun)) 
            return [];
        
        var bonusValue = 0.3;
            
        if (Stats[Affix.CritRate] >= 0.8) {
            bonusValue *= 1.25;
        }

        return [new(bonusValue, Affix.DmgBonus, [SkillTag.Chain, SkillTag.Ultimate])];

    }
}