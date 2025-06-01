using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest {
    private CalcRequest Jane { get; } = new() {
        AgentId = 1261,
        WeaponId = 14126,
        Discs = [
            new () {
                SetId = 31300, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.AnomalyProficiency, Affix.HpRatio, Affix.DefRatio, Affix.CritDamage],
                Levels = [15, 3, 2, 1, 3]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AnomalyProficiency, Affix.HpRatio, Affix.AtkRatio, Affix.CritRate],
                Levels = [15, 3, 1, 3, 1]
            },
            new () {
                SetId = 32600,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.AtkRatio, Affix.AnomalyProficiency, Affix.Atk],
                Levels = [15, 1, 3, 2, 2]
            },
            new () {
                SetId = 32600,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyProficiency, Affix.CritRate, Affix.CritDamage, Affix.Hp, Affix.HpRatio],
                Levels = [15, 4, 2, 1, 2]
            },
            new () {
                SetId = 32600,
                Rarity = Rarity.S,
                Stats = [Affix.PhysicalDmgBonus, Affix.CritDamage, Affix.HpRatio, Affix.AnomalyProficiency, Affix.Pen],
                Levels = [15, 1, 2, 2, 3]
            },
            new () {
                SetId = 32600,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyMasteryRatio, Affix.HpRatio, Affix.CritRate, Affix.AtkRatio, Affix.DefRatio],
                Levels = [15, 2, 2, 3, 1]
            },
        ],
        Team = [],
        StunBonus = 1.5,
        Rotation = [
            "flowers_of_sin",
            "phantom_thrust",
            "salchow_jump 1",
            "salchow_jump 2",
            "aerial_sweep_cleanout",
            "aerial_sweep_cleanout",
            "phantom_thrust",
            "final_curtain"
        ]
    };

    [Test]
    public void JaneTest() {
        var result = Calculator.Calculate(Jane.AgentId, Jane.WeaponId, GetDriveDiscs(Jane), Jane.Team, Jane.Rotation);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Assault triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
    }
}