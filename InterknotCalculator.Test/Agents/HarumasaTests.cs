using System.Collections.Immutable;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

public class HarumasaTests : AgentsTest {
    protected override CalcRequest Request { get; } = new() {
        AgentId = AgentId.Harumasa,
        WeaponId = WeaponId.ZanshinHerbCase,
        Discs = [
            new () {
                SetId = DriveDiscSetId.HormonePunk, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.Pen, Affix.AtkRatio, Affix.CritDamage, Affix.CritRate],
                Levels = [15, 2, 3, 3, 1]
            },
            new () {
                SetId = DriveDiscSetId.ThunderMetal, 
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AtkRatio, Affix.Pen, Affix.CritDamage, Affix.CritRate],
                Levels = [15, 2, 3, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.ThunderMetal, 
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AtkRatio, Affix.CritRate, Affix.CritDamage, Affix.Pen],
                Levels = [15, 4, 2, 2, 1]
            },
            new () {
                SetId = DriveDiscSetId.ThunderMetal, 
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.Pen, Affix.CritRate, Affix.Atk, Affix.AtkRatio],
                Levels = [15, 2, 3, 1, 3]
            },
            new () {
                SetId = DriveDiscSetId.HormonePunk, 
                Rarity = Rarity.S,
                Stats = [Affix.ElectricDmgBonus, Affix.CritRate, Affix.Atk, Affix.Pen, Affix.CritDamage],
                Levels = [15, 4, 1, 2, 1]
            },
            new () {
                SetId = DriveDiscSetId.ThunderMetal, 
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.Def, Affix.CritDamage, Affix.Pen, Affix.CritRate],
                Levels = [15, 1, 3, 1, 3]
            },
        ],
        Team = [],
        Enemy = new(nameof(NotoriousDullahan), 1.5, 
            new(Element.Electric, 0, new())),
        Rotation = [
            "kai_hanare",
            "falling_feather",
            "hiten_no_tsuru_slash 1",
            "hiten_no_tsuru_slash 2",
            "hiten_no_tsuru_slash 3",
            "hiten_no_tsuru_slash 2",
            "cloud_piercer 4",
            "cloud_piercer 5",
            "falling_feather",
            "hiten_no_tsuru_slash 1",
            "hiten_no_tsuru_slash 2",
            "hiten_no_tsuru_slash 3",
            "hiten_no_tsuru_slash 2",
            "nowhere_to_run",
            "falling_feather",
            "hiten_no_tsuru_slash 1",
            "hiten_no_tsuru_slash 2",
            "hiten_no_tsuru_slash 3",
            "hiten_no_tsuru_slash 2",
            "zanshin"
        ]
    };
    
    [Test]
    public async Task Harumasa12SlashTest() {
        var result = Calculator.Calculate(Request);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        AssertRotationOrderPreserved(Request.Rotation, result.PerAction.ToImmutableList(), Request.AgentId);
        
        await VerifyActions(result.PerAction);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
    }
}