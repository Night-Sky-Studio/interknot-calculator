using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class Soldier11 : Agent {
    public Soldier11() {
        Speciality = Speciality.Attack;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.NewEriduDefenseForce;

        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 813 + 75;
        Stats[Affix.CritRate] = 0.05 + 0.144;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 93;
        Stats[Affix.AnomalyMastery] = 94;
        Stats[Affix.AnomalyProficiency] = 93;
        
        Skills["warming_sparks"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(69.6, 26, Element.Physical),
                new(83, 52, Element.Physical),
                new(206.2, 124.1, Element.Physical),
                new(426.8, 252.3, Element.Physical)
            ]
        };
        Skills["fire_suppression_basic"] = new() {
            Tag = SkillTag.BasicAtk,
            Affixes = {
                { Affix.DmgBonus, 0.7 } // Heatwave passive
            },
            Scales = [
                new(111.2, 27.9),
                new(114.4, 51.2),
                new(264, 113.7),
                new(681.7, 287.7)
            ]
        };

        Skills["blazing_fire"] = new() {
            Tag = SkillTag.Dash,
            Scales = [
                new(137.6, 51.8, Element.Physical)
            ]
        };
        Skills["fire_suppression_dash"] = new() {
            Tag = SkillTag.Dash,
            Affixes = {
                { Affix.DmgBonus, 0.7 } // Heatwave passive
            },
            Scales = [
                new(158, 118.4)
            ]
        };
        Skills["backdraft"] = new() {
            Tag = SkillTag.Counter,
            Scales = [
                new(524.9, 339.1)
            ]
        };

        Skills["covering_fire"] = new() {
            Tag = SkillTag.QuickAssist,
            Scales = [
                new(241.8, 181.3)
            ]
        };
        Skills["hold_the_line"] = new() {
            Tag = SkillTag.DefensiveAssist,
            Scales = [
                new(0, 388.8),
                new(0, 491.2),
                new(0, 239.6)
            ]
        };
        Skills["reignition"] = new() {
            Tag = SkillTag.FollowUpAssist,
            Scales = [
                new (767.6, 503.8)
            ]
        };

        Skills["raging_fire"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new(105.4, 79)
            ]
        };
        Skills["fervent_fire"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(1350.4, 816.3)
            ]
        };

        Skills["uplifting_flame"] = new() {
            Tag = SkillTag.Chain,
            Scales = [
                new(1265, 321.4)
            ]
        };
        Skills["bellowing_flame"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [
                new(4206.2, 428)
            ]
        };
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(0.325, Affix.FireDmgBonus)];
        }

        return [];
    }
}