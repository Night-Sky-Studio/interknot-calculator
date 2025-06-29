using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest {
    private CalcRequest Burnice { get; } = new() {
        AgentId = 1171,
        WeaponId = 14117,
        Discs = [
            new () {
                SetId = 31300, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.DefRatio, Affix.AtkRatio, Affix.Atk, Affix.AnomalyProficiency],
                Levels = [15, 2, 1, 3, 2]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.CritDamage, Affix.Pen, Affix.AnomalyProficiency],
                Levels = [15, 3, 3, 1, 2]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AtkRatio, Affix.AnomalyProficiency, Affix.CritDamage, Affix.DefRatio],
                Levels = [15, 1, 1, 3, 3]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyProficiency, Affix.CritDamage, Affix.Pen, Affix.DefRatio, Affix.Hp],
                Levels = [15, 3, 1, 2, 3]
            },
            new () {
                SetId = 31800,
                Rarity = Rarity.S,
                Stats = [Affix.FireDmgBonus, Affix.Atk, Affix.DefRatio, Affix.Hp, Affix.Pen],
                Levels = [15, 1, 2, 2, 3]
            },
            new () {
                SetId = 31300,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyMasteryRatio, Affix.AnomalyProficiency, Affix.Atk, Affix.Def, Affix.HpRatio],
                Levels = [15, 4, 1, 2, 2]
            },
        ],
        Team = [1151],
        StunBonus = 1.5,
        Rotation = [
            "fuel-fed_flame",
            "intense_heat_stirring_method_double",
            "glorious_inferno",
            "energizing_speciality_drink"
        ]
    };

    [Test]
    public void BurniceTest() {
        var enemy = new NotoriousDullahan();
        var result = Calculator.Calculate(Burnice.AgentId, Burnice.WeaponId, GetDriveDiscs(Burnice), 
            Burnice.Team, Burnice.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction);
        Console.WriteLine($"Total: {result.Total}");
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', enemy.AnomalyBuildup)}");
    }
}