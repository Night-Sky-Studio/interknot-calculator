using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Miyabi : Agent, ICustomAnomaly {
    public Element AnomalyElement { get; set; }
    
    public Miyabi() : base(1091) {
        Speciality = Speciality.Anomaly;
        Element = Element.Ice;
        AnomalyElement = Element.Frost;
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
        Stats[Affix.EnergyRegen] = 1.2;

        Anomalies[Element.Frost] = new(1500, Element.Frost, []);

        Skills["kazahana"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new (54.4, 21.2, element: Element.Physical),
                new (59.3, 41.2, element: Element.Physical),
                new (126.6, 70.6, 62.9),
                new (193.3, 123.9, 91.21),
                new (258.8, 197.0, 129.27)
            ]
        };
        Skills["shimotsuki"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(910.1, 66.0, 39.98),
                new(1717.2, 97.2, 56.68),
                new(4282.8, 567.0, 343.36),
            ],
            Affixes = {
                { Affix.DmgBonus, 0.6 },
                { Affix.IceResPen, 0.3 }
            }
        };
        
        Skills["fuyubachi"] = new() {
            Tag = SkillTag.Dash,
            Scales = [new(52.2, 19.5, element: Element.Physical)]
        };
        Skills["kan_suzume"] = new() {
            Tag = SkillTag.Counter,
            Scales = [new(492.3, 322.3, 94.99)]
        };
        
        Skills["dancing_petals"] = new() {
            Tag = SkillTag.QuickAssist,
            Scales = [new(209.0, 157.3, 94.99)]
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
                new (676.6, 438.2, 286.17)
            ]
        };
        
        Skills["miyuki"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new (72.1, 54.5, 32.51)
            ]
        };
        Skills["hisetsu"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(788.3, 483.4, 157.75 + 250.52),
                new(967.2, 608.5, 189.25 + 297.77)
            ]
        };
        
        Skills["springs_call"] = new() {
            Tag = SkillTag.Chain,
            Scales = [new(1258.3, 284.7, 111.36 * 2 + 148.48)]
        };
        Skills["lingering_snow"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [new(4776.1, 556.3, 1137.16)]
        };
    }

    public override void ApplyPassive() {
        BonusStats[Affix.IceDmgBonus] += 0.3;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Support) ||
            team.Any(a => a.Faction == Faction)) {
            return [
                new (Affix.DmgBonus, 0.6, tags: [SkillTag.BasicAtk]),
                new (Affix.IceResPen, 0.3, tags: [SkillTag.BasicAtk])
            ];
        }

        return [];
    }

    // public override AgentAction GetAnomalyDamage(Element element, Enemy enemy) {
    //     base.GetAnomalyDamage(element, enemy);
    // }
}