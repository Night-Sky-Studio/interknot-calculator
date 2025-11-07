using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class ZhuYuanTests : AgentsTest {
    private CalcRequest ZhuYuan { get; } = new() {
        AgentId = AgentId.ZhuYuan,
        WeaponId = WeaponId.RiotSuppressorMarkVI,
        Discs = [
            new () {
                SetId = DriveDiscSetId.ChaoticMetal, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritRate, Affix.CritDamage, Affix.Atk, Affix.AtkRatio],
                Levels = [15, 3, 2, 1, 2]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AtkRatio, Affix.Pen, Affix.CritDamage, Affix.CritRate],
                Levels = [15, 1, 2, 2, 4]
            },
            new () {
                SetId = DriveDiscSetId.ChaoticMetal,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.Atk, Affix.AtkRatio, Affix.CritDamage, Affix.HpRatio],
                Levels = [15, 2, 2, 1, 3]
            },
            new () {
                SetId = 32300,
                Rarity = Rarity.S,
                Stats = [Affix.CritRate, Affix.Pen, Affix.Hp, Affix.Atk, Affix.CritDamage],
                Levels = [15, 3, 1, 2, 3]
            },
            new () {
                SetId = 31000,
                Rarity = Rarity.S,
                Stats = [Affix.EtherDmgBonus, Affix.HpRatio, Affix.CritRate, Affix.CritDamage, Affix.AnomalyProficiency],
                Levels = [15, 1, 3, 3, 1]
            },
            new () {
                SetId = 32300,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.Pen, Affix.DefRatio, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 2, 1, 2, 3]
            },
        ],
        Team = [],
        StunBonus = 1.5,
        Rotation = [
            "eradication_mode",
            "covering_shot",
            "full_barrage",
            "please_do_not_resist 1",
            "please_do_not_resist 2",
            "please_do_not_resist 3",
            "overwhelming_firepower",
            "please_do_not_resist 3",
            "full_barrage",
            "max_eradication_mode",
            "please_do_not_resist 1",
            "please_do_not_resist 2",
            "please_do_not_resist 3",
            "overwhelming_firepower",
            "please_do_not_resist 3",
            "full_barrage"
        ]
    };

    [Test]
    public void ZhuYuanTest() {
        var enemy = new NotoriousDullahan();
        var result = Calculator.Calculate(ZhuYuan.AgentId, ZhuYuan.WeaponId, GetDriveDiscs(ZhuYuan), 
            ZhuYuan.Team, ZhuYuan.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', enemy.AnomalyBuildup)}");
    }
}