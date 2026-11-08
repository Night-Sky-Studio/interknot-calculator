using System.Collections.Immutable;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class Soldier11Tests : AgentsTest {
    protected override CalcRequest Request { get; } = new() {
        AgentId = AgentId.Soldier11,
        WeaponId = WeaponId.TheBrimstone,
        Discs = [
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritDamage, Affix.CritRate, Affix.Atk, Affix.AtkRatio],
                Levels = [15, 2, 3, 1, 2]
            },
            new () {
                SetId = DriveDiscSetId.InfernoMetal,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.Def, Affix.AtkRatio, Affix.CritDamage],
                Levels = [15, 2, 1, 3, 3]
            },
            new () {
                SetId = DriveDiscSetId.InfernoMetal,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.CritRate, Affix.Atk, Affix.Pen],
                Levels = [15, 4, 3, 1, 1]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.AnomalyProficiency, Affix.AtkRatio, Affix.Atk, Affix.CritRate],
                Levels = [15, 1, 2, 2, 3]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.FireDmgBonus, Affix.DefRatio, Affix.AtkRatio, Affix.CritDamage, Affix.CritRate],
                Levels = [15, 1, 3, 2, 3]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.DefRatio, Affix.Atk, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 1, 2, 3, 2]
            },
        ],
        Team = [],
        Enemy = new(nameof(NotoriousDullahan), 1.5, 
            new(Element.Fire, 0, new())),
        Rotation = [
            "uplifting_flame",
            "covering_fire",
            "fire_suppression_basic 1",
            "fire_suppression_basic 2",
            "fire_suppression_basic 3",
            "fire_suppression_basic 4",
            "fervent_fire",
            "bellowing_flame",
            "fire_suppression_basic 1",
            "fire_suppression_basic 2",
            "fire_suppression_basic 3",
            "fire_suppression_basic 4",
            "fire_suppression_basic 1",
            "fire_suppression_basic 2",
            "fire_suppression_basic 3",
            "fire_suppression_basic 4"
        ]
    };

    [Test]
    public async Task Soldier11Test() {
        var result = Calculator.Calculate(Request);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        AssertRotationOrderPreserved(Request.Rotation, result.PerAction.ToImmutableList(), Request.AgentId);
        
        await VerifyActions(result.PerAction);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
}
