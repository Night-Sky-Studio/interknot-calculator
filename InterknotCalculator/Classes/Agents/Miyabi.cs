using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class Miyabi : Agent {
    public Miyabi() {
        Speciality = Speciality.Anomaly;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.HollowSpecialOperationsSection6;

        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 880;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 116;
        Stats[Affix.AnomalyProficiency] = 238;
        Stats[Affix.IceDmgBonus] = 0.3;

        Anomalies["frostburn"] = new(1500, Element.Ice, []);

        Skills["kazahana"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new (54.4, 21.2, Element.Physical),
                new (59.3, 41.2, Element.Physical),
                new (126.6, 70.6),
                new (193.3, 123.9),
                new (258.8, 197.0)
            ]
        };
        Skills["shimotsuki"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(910.1, 66.0),
                new(1717.2, 97.2),
                new(4282.8, 567.0),
            ],
            Affixes = {
                { Affix.DmgBonus, 0.6 },
                { Affix.IceResPen, 0.3 }
            }
        };
        Skills["fuyubachi"] = new() {
            Tag = SkillTag.Dash,
            Scales = [new(52.2, 19.5, Element.Physical)]
        };
        Skills["kan_suzume"] = new() {
            Tag = SkillTag.Counter,
            Scales = [new(492.3, 322.3)]
        };
        Skills["dancing_petals"] = new() {
            Tag = SkillTag.QuickAssist,
            Scales = [new(209.0, 157.3)]
        };
        Skills["drifting_petals"] = new() {
            Tag = SkillTag.DefensiveAssist,
            Scales = [
                new (0, 407.7),
                new (0, 514.4),
                new (0, 193.2)
            ]
        };
        Skills["falling_petals"] = new() {
            Tag = SkillTag.FollowUpAssist,
            Scales = [
                new (676.6, 438.2)
            ]
        };
        Skills["miyuki"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new (72.1, 54.5)
            ]
        };
        Skills["hisetsu"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(788.3, 483.4),
                new(967.2, 608.5)
            ]
        };
        Skills["springs_call"] = new() {
            Tag = SkillTag.Chain,
            Scales = [new(1258.3, 284.7)]
        };
        Skills["lingering_snow"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [new(4776.1, 556.3)]
        };
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count == 0) return [];

        if (team.Any(a => a.Speciality == Speciality.Support) ||
            team.Any(a => a.Faction == Faction)) {
            return [
                new (0.6, Affix.DmgBonus, [SkillTag.BasicAtk]),
                new(0.3, Affix.IceResPen, [SkillTag.BasicAtk])
            ];
        }

        return [];
    }
}