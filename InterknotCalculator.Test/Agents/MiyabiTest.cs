using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest {
    private CalcRequest Miyabi { get; } = new() {
        AgentId = 1091,
        WeaponId = 14109,
        Discs = [
            new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritRate, Affix.AtkRatio, Affix.CritDamage, Affix.HpRatio],
                Levels = [15, 3, 2, 3, 1]
            },
            new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.AtkRatio, Affix.CritDamage, Affix.AnomalyProficiency],
                Levels = [15, 3, 4, 1, 1]
            },
            new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.AtkRatio, Affix.CritRate, Affix.Pen],
                Levels = [15, 2, 4, 2, 1]
            },
            new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.DefRatio, Affix.AtkRatio, Affix.CritRate, Affix.Atk],
                Levels = [15, 1, 3, 3, 2]
            },
            new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.IceDmgBonus, Affix.AtkRatio, Affix.HpRatio, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 3, 1, 3, 2]
            },
            new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.CritDamage, Affix.CritRate, Affix.Atk, Affix.Pen],
                Levels = [15, 2, 4, 1, 2]
            }
            /*new () {
                SetId = 32700, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen],
                Levels = [15, 5, 2, 1, 1]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen],
                Levels = [15, 2, 1, 5, 1]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Atk],
                Levels = [15, 5, 2, 1, 1]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen, Affix.Atk],
                Levels = [15, 4, 3, 1, 1]
            },
            new () {
                SetId = 31000,
                Rarity = Rarity.S,
                Stats = [Affix.IceDmgBonus, Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Pen],
                Levels = [15, 1, 2, 5, 1]
            },
            new () {
                SetId = 31000,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.CritRate, Affix.CritDamage, Affix.Pen, Affix.Atk],
                Levels = [15, 5, 2, 1, 1]
            },*/
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
        var enemy = new NotoriousDullahan {
            StunMultiplier = Miyabi.StunBonus
        };
        var result = Calculator.Calculate(Miyabi.AgentId, Miyabi.WeaponId, GetDriveDiscs(Miyabi), 
            Miyabi.Team, Miyabi.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Assert.That(result.PerAction.Any(action => action.Name == "frostburn"), Is.True);
        Assert.That(result.PerAction.Any(action => action.Name == "shatter"), Is.True);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction);
        Console.WriteLine($"Total: {result.Total}");
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', enemy.AnomalyBuildup)}");
    } 
}