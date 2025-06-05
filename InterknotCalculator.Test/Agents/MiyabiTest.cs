using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest {
    private CalcRequest Miyabi { get; } = new() {
        AgentId = 1091,
        WeaponId = 14109,
        Discs = [
            new () {
                SetId = 31000, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.HpRatio, Affix.CritDamage, Affix.CritRate, Affix.AtkRatio],
                Levels = [15, 2, 1, 3, 3]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AnomalyProficiency, Affix.CritDamage, Affix.CritRate, Affix.Pen],
                Levels = [15, 1, 3, 3, 1]
            },
            new () {
                SetId = 31000,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AtkRatio, Affix.CritRate, Affix.CritDamage, Affix.Atk],
                Levels = [15, 2, 4, 1, 2]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.AtkRatio, Affix.Pen, Affix.CritRate, Affix.Atk],
                Levels = [15, 1, 3, 3, 2]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.PenRatio, Affix.HpRatio, Affix.CritDamage, Affix.AtkRatio, Affix.Hp],
                Levels = [15, 2, 4, 1, 1]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.Hp, Affix.CritDamage, Affix.Atk, Affix.CritRate],
                Levels = [15, 2, 2, 3, 1]
            },
        ],
        Team = [],
        StunBonus = 1.5,
        Rotation = [
            "hisetsu 1",
            "hisetsu 2",
            "kazahana 3",
            "kazahana 4",
            "kazahana 5",
            "shimotsuki 3",
            "springs_call",
            "lingering_snow",
            "shimotsuki 3"
        ]
    };

    [Test]
    public void MiyabiTest() {
        var result = Calculator.Calculate(Miyabi.AgentId, Miyabi.WeaponId, GetDriveDiscs(Miyabi), Miyabi.Team, Miyabi.Rotation);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Assault triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        Console.WriteLine($"Enemy Anomaly Buildup: {result.Enemy?.AnomalyBuildup}");
    } 
}