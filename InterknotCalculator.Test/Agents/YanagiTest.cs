using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class YanagiTests : AgentsTest {
    private CalcRequest Yanagi { get; } = new() {
        AgentId = 1221,
        WeaponId = 14122,
        Discs = [
            new () {
                SetId = 31300, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.AnomalyProficiency, Affix.HpRatio, Affix.CritDamage, Affix.Pen],
                Levels = [15, 4, 1, 1, 3]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.AnomalyProficiency, Affix.Def, Affix.CritDamage],
                Levels = [15, 2, 3, 2, 1]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AnomalyProficiency, Affix.CritDamage, Affix.Pen, Affix.AtkRatio],
                Levels = [15, 3, 1, 2, 2]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyProficiency, Affix.Def, Affix.DefRatio, Affix.CritDamage, Affix.HpRatio],
                Levels = [15, 2, 2, 3, 1]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.PenRatio, Affix.CritRate, Affix.AnomalyProficiency, Affix.Atk, Affix.DefRatio],
                Levels = [15, 2, 3, 2, 1]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyMasteryRatio, Affix.CritDamage, Affix.Hp, Affix.CritRate, Affix.AtkRatio],
                Levels = [15, 3, 2, 2, 2]
            },
        ],
        Team = [new(1171)],
        StunBonus = 1,
        Rotation = [
            "1171.energizing_speciality_drink",
            "1171.intense_heat_stirring_method",
            "1171.intense_heat_stirring_method_double",
            "1171.fuel-fed_flame",
            "gekka_ruten",
            "gekka_ruten 2",
            "gekka_ruten",
            "gekka_ruten 2",
            "gekka_ruten",
            "gekka_ruten 2"
        ]
    };

    [Test]
    public void YanagiTest() {
        var enemy = new NotoriousDullahan {
            AfflictedAnomaly = Anomaly.GetAnomalyByElement(Element.Fire)! with {
                AgentId = 1151,
                Stats = new() {
                    [Affix.Atk] = 2500,
                    [Affix.AnomalyProficiency] = 372
                }
            }
        };
        var result = Calculator.Calculate(Yanagi, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        // has polarity_disorder and dmg is > 0
        Assert.That(result.PerAction, Has.Exactly(3).Matches<AgentAction>(action => action is {
            Name: "polarity_disorder", Damage: > 0
        }));
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
}