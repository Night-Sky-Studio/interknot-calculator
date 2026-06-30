using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class EvelynTests : AgentsTest {
    private CalcRequest Evelyn { get; } = new() {
        AgentId = AgentId.Evelyn,
        WeaponId = WeaponId.HeartstringNocturne,
        Discs = [
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.AnomalyProficiency, Affix.AtkRatio, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 1, 3, 2, 3]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.DefRatio, Affix.AtkRatio, Affix.CritDamage],
                Levels = [15, 1, 1, 3, 3]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AnomalyProficiency, Affix.Pen, Affix.CritDamage, Affix.AtkRatio],
                Levels = [15, 1, 1, 5, 2]
            },
            new () {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.CritRate, Affix.Pen, Affix.AtkRatio, Affix.Hp],
                Levels = [15, 3, 2, 3, 1]
            },
            new () {
                SetId = DriveDiscSetId.BranchBladeSong,
                Rarity = Rarity.S,
                Stats = [Affix.PenRatio, Affix.Pen, Affix.AtkRatio, Affix.CritRate, Affix.Atk],
                Levels = [15, 1, 4, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.BranchBladeSong,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.DefRatio, Affix.AnomalyProficiency, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 1, 1, 2, 5]
            },
        ],
        Team = [
            new(AgentId.JuFufu, WeaponId.RoaringFurnace, DriveDiscSetId.KingOfTheSummit), 
            new(AgentId.AstraYao, WeaponId.ElegantVanity, DriveDiscSetId.AstralVoice)
        ],
        StunBonus = 1.5,
        Rotation = [
            "locked_position",
            "binding_sunder_first",
            "razor_wire 3",
            "razor_wire 4",
            "razor_wire 5",
            "binding_sunder_second",
            "garrote 1",
            "garrote 2",
            "lunalux",
            "lunalux",
            "lunalux_garrote",
            "binding_sunder_second",
            "garrote 1",
            "lunalux"
        ]
    };

    [Test]
    public void EvelynTest() {
        var result = Calculator.Calculate(Evelyn);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
}
