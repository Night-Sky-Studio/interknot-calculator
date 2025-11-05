using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class YixuanTests: AgentsTest {
    private CalcRequest Yixuan { get; } = new() {
        AgentId = AgentId.Yixuan,
        WeaponId = WeaponId.QingmingBirdcage,
        Discs = [
            new () {
                SetId = DriveDiscSetId.YunkuiTales, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritRate, Affix.HpRatio, Affix.CritDamage, Affix.Def],
                Levels = [15, 2, 3, 2, 1]
            },
            new () {
                SetId = DriveDiscSetId.BranchBladeSong,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AtkRatio, Affix.CritDamage, Affix.Hp, Affix.Def],
                Levels = [15, 2, 4, 2, 1]
            },
            new () {
                SetId = DriveDiscSetId.YunkuiTales,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.CritRate, Affix.HpRatio, Affix.AtkRatio],
                Levels = [15, 2, 2, 3, 1]
            },
            new () {
                SetId = DriveDiscSetId.BranchBladeSong,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.Hp, Affix.HpRatio, Affix.CritRate, Affix.Pen],
                Levels = [15, 2, 2, 4, 1]
            },
            new () {
                SetId = DriveDiscSetId.YunkuiTales,
                Rarity = Rarity.S,
                Stats = [Affix.EtherDmgBonus, Affix.Def, Affix.CritRate, Affix.CritDamage, Affix.Hp],
                Levels = [15, 2, 2, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.YunkuiTales,
                Rarity = Rarity.S,
                Stats = [Affix.HpRatio, Affix.Atk, Affix.CritRate, Affix.CritDamage, Affix.AtkRatio],
                Levels = [15, 2, 1, 3, 2]
            },
        ],
        Team = [],
        StunBonus = 1.5,
        Rotation = [
            "auric_ink_rush",
            "ink_manifestation_charged",
            "cloud_shaper",
            "ashen_ink_becomes_shadows",
            "endless_talisman_suppression",
            "qingming_skyshade",
            "auric_array",
            "qingming_eruption"
        ]
    };

    [Test]
    public void YixuanTest() {
        var enemy = new NotoriousDullahan {
            StunMultiplier = Yixuan.StunBonus
        };
        var result = Calculator.Calculate(Yixuan.AgentId, Yixuan.WeaponId, GetDriveDiscs(Yixuan), 
            Yixuan.Team, Yixuan.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        // Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', enemy.AnomalyBuildup)}");
    }
}