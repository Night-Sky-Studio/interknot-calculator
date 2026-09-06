using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class EllenTests : AgentsTest {
    protected override CalcRequest Request { get; } = new() {
        AgentId = AgentId.Ellen,
        WeaponId = WeaponId.DeepSeaVisitor,
        // Discs = [
        //     new () {
        //         SetId = DriveDiscSetId.PolarMetal, 
        //         Rarity = Rarity.S,
        //         Stats = [Affix.Hp, Affix.Pen, Affix.CritDamage, Affix.CritRate, Affix.AtkRatio],
        //         Levels = [15, 2, 2, 3, 2]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.WoodpeckerElectro,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.Atk, Affix.CritDamage, Affix.DefRatio, Affix.CritRate, Affix.AnomalyProficiency],
        //         Levels = [15, 4, 1, 2, 1]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.WoodpeckerElectro,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.Def, Affix.CritDamage, Affix.CritRate, Affix.AnomalyProficiency, Affix.HpRatio],
        //         Levels = [15, 5, 1, 1, 1]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.PolarMetal,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.CritDamage, Affix.CritRate, Affix.AtkRatio, Affix.Pen, Affix.Hp],
        //         Levels = [15, 1, 2, 3, 3]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.WoodpeckerElectro,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.PenRatio, Affix.Atk, Affix.CritDamage, Affix.CritRate, Affix.HpRatio],
        //         Levels = [15, 3, 2, 3, 1]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.WoodpeckerElectro,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.AtkRatio, Affix.CritDamage, Affix.CritRate, Affix.Def, Affix.AnomalyProficiency],
        //         Levels = [15, 4, 2, 1, 1]
        //     },
        // ],
        Discs = [
            new () {
                SetId = DriveDiscSetId.PufferElectro, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.Pen, Affix.CritRate, Affix.CritDamage, Affix.Atk],
                Levels = [15, 1, 3, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritDamage, Affix.DefRatio, Affix.CritRate, Affix.AnomalyProficiency],
                Levels = [15, 4, 1, 2, 1]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.CritRate, Affix.AnomalyProficiency, Affix.HpRatio],
                Levels = [15, 5, 1, 1, 1]
            },
            new () {
                SetId = DriveDiscSetId.PufferElectro,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.AnomalyProficiency, Affix.CritRate, Affix.CritDamage, Affix.Pen],
                Levels = [15, 2, 1, 3, 2]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.PenRatio, Affix.Atk, Affix.CritDamage, Affix.CritRate, Affix.HpRatio],
                Levels = [15, 3, 2, 3, 1]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.CritDamage, Affix.CritRate, Affix.Def, Affix.AnomalyProficiency],
                Levels = [15, 4, 2, 1, 1]
            },
        ],
        Team = [new(AgentId.Lycaon), new(AgentId.Rina)],
        StunBonus = 1.5,
        Rotation = [
            "avalanche",
            "tail_swipe",
            "flash_freeze_trimming 3",
            "endless_winter",
            "tail_swipe",
            "sharknami",
            "flash_freeze_trimming 3"
        ]
    };

    [Test]
    public async Task EllenTest() {
        var result = Calculator.Calculate(Request);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        foreach (var action in Request.Rotation) {
            Assert.That(result.PerAction, Has.Some.Matches<AgentAction>(a => 
                a.Name.Contains(action)));
        }
        
        await VerifyActions(result.PerAction);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
}

[TestFixture]
public class EllenM6Tests : AgentsTest {
    protected override CalcRequest Request { get; } = new() {
        AgentId = AgentId.Ellen,
        WeaponId = WeaponId.DeepSeaVisitor,
        Discs = [
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.AnomalyProficiency, Affix.CritDamage, Affix.CritRate, Affix.AtkRatio],
                Levels = [15, 3, 2, 2, 2]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.DefRatio, Affix.AtkRatio, Affix.CritDamage, Affix.CritRate],
                Levels = [15, 1, 3, 3, 2]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritRate, Affix.HpRatio, Affix.CritDamage, Affix.Atk],
                Levels = [15, 3, 1, 4, 1]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.AtkRatio, Affix.Atk, Affix.HpRatio, Affix.CritRate],
                Levels = [15, 3, 2, 2, 1]
            },
            new() {
                SetId = DriveDiscSetId.PolarMetal,
                Rarity = Rarity.S,
                Stats = [Affix.IceDmgBonus, Affix.CritDamage, Affix.Atk, Affix.AtkRatio, Affix.CritRate],
                Levels = [15, 2, 2, 3, 1]
            },
            new() {
                SetId = DriveDiscSetId.PolarMetal,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.Hp, Affix.Pen, Affix.CritDamage, Affix.CritRate],
                Levels = [15, 2, 3, 2, 2]
            },
        ],
        Mindscape = 6,
        Team = [],//[new(AgentId.Lycaon)],
        StunBonus = 1.5,
        Rotation = [
            "avalanche",
            "endless_winter",
            "tail_swipe",
            "sharknami",
            "icy_blade 2",
            "flash_freeze_trimming 3",
            "icy_blade 2",
            "sharknami",
            "icy_blade 2",
            "flash_freeze_trimming 3",
            "icy_blade 2"
        ]
    };
    
    [Test]
    public async Task EllenM6Test() {
        var result = Calculator.Calculate(Request);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        foreach (var action in Request.Rotation) {
            var act = RotationAction.Parse(action, AgentId.Ellen);
            if (act is null) {
                Assert.Fail($"Failed to parse action: {action}");
                return;
            }

            Assert.That(result.PerAction, Has.Some.Matches<AgentAction>(a =>
                a.Name.Contains(act.ActionName)));
        }
        
        await VerifyActions(result.PerAction);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
    
}