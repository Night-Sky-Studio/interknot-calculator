using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest {
    private CalcRequest Grace { get; } = new() {
        AgentId = 1181,
        WeaponId = 14118,
        Discs = [
            new () {
                SetId = 32400, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.HpRatio, Affix.AnomalyProficiency, Affix.DefRatio, Affix.Pen],
                Levels = [15, 2, 3, 1, 2]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.AtkRatio, Affix.AnomalyProficiency, Affix.DefRatio],
                Levels = [15, 2, 3, 2, 1]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritRate, Affix.HpRatio, Affix.AtkRatio, Affix.CritDamage],
                Levels = [15, 2, 1, 5, 1]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyProficiency, Affix.CritDamage, Affix.CritRate, Affix.HpRatio, Affix.AtkRatio],
                Levels = [15, 2, 1, 2, 3]
            },
            new () {
                SetId = 32400,
                Rarity = Rarity.S,
                Stats = [Affix.ElectricDmgBonus, Affix.CritRate, Affix.HpRatio, Affix.AtkRatio, Affix.Pen],
                Levels = [15, 2, 1, 3, 2]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyMasteryRatio, Affix.AnomalyProficiency, Affix.Def, Affix.Hp, Affix.HpRatio],
                Levels = [15, 3, 1, 2, 2]
            },
        ],
        Team = [1211],
        StunBonus = 1.5,
        Rotation = [
            "collaborative_construction",
            "high-pressure_spike 1",
            "high-pressure_spike 2",
            "high-pressure_spike 3",
            "supercharged_obstruction_removal",
            "high-pressure_spike 4",
            "supercharged_obstruction_removal",
            "demolition_blast"
        ]
    };

    [Test]
    public void GraceTest() {
        var enemy = new NotoriousDullahan();
        var result = Calculator.Calculate(Grace.AgentId, Grace.WeaponId, GetDriveDiscs(Grace), 
            Grace.Team, Grace.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction);
        Console.WriteLine($"Total: {result.Total}");
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', enemy.AnomalyBuildup)}");
    }
}