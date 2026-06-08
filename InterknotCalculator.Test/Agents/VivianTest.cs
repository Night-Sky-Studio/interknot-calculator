using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class VivianTests : AgentsTest {
    private CalcRequest Vivian { get; } = new CalcRequest {
        AgentId = 1331,
        WeaponId = 14133,
        Discs = [
            new () {
                SetId = 33000, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.AnomalyProficiency, Affix.Pen, Affix.CritDamage, Affix.DefRatio],
                Levels = [15, 2, 3, 1, 2]
            },
            new () {
                SetId = 33000,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritDamage, Affix.AnomalyProficiency, Affix.AtkRatio, Affix.Hp],
                Levels = [15, 2, 3, 1, 2]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.AnomalyProficiency, Affix.CritRate, Affix.Hp],
                Levels = [15, 3, 1, 2, 2]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyProficiency, Affix.CritRate, Affix.Pen, Affix.CritDamage, Affix.DefRatio],
                Levels = [15, 2, 2, 2, 2]
            },
            new () {
                SetId = 33000,
                Rarity = Rarity.S,
                Stats = [Affix.EtherDmgBonus, Affix.Def, Affix.Atk, Affix.Pen, Affix.CritRate],
                Levels = [15, 3, 4, 1, 1]
            },
            new () {
                SetId = 33000,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyMasteryRatio, Affix.HpRatio, Affix.CritDamage, Affix.AnomalyProficiency, Affix.CritRate],
                Levels = [15, 3, 3, 1, 1]
            },
        ],
        Team = [new(AgentId.Jane)],
        StunBonus = 1,
        Rotation = [
            "1261.aerial_sweep_cleanout",
            "1261.aerial_sweep_cleanout",
            "violet_requiem",
            "fluttering_frock_suspension",
            "1261.aerial_sweep_cleanout",
            "1261.aerial_sweep_cleanout"
        ]
    };
    
    [Test]
    public void VivianTest() {
        var result = Calculator.Calculate(Vivian);
        
        Assert.That(result.PerAction, Is.Not.Empty);

        Assert.That(result.PerAction, Has.Exactly(3).Matches<AgentAction>(a => a is {
            Tag: SkillTag.AttributeAnomaly,
            Name: "abloom_assault"
        }));
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
}