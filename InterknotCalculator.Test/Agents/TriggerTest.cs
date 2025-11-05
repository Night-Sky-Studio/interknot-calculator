using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public class TriggerTests : AgentsTest {
    private CalcRequest Trigger { get; } = new() {
        AgentId = AgentId.Trigger,
        WeaponId = WeaponId.SpectralGaze,
        Discs = [
            new() {
                SetId = DriveDiscSetId.ShadowHarmony,
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritRate, Affix.Def, Affix.CritDamage, Affix.AtkRatio],
                Levels = [15, 4, 2, 1, 2]
            },
            new() {
                SetId = DriveDiscSetId.ShadowHarmony,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.Pen, Affix.CritDamage, Affix.AtkRatio],
                Levels = [15, 3, 1, 2, 3]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritRate, Affix.AnomalyProficiency, Affix.CritDamage, Affix.Pen],
                Levels = [15, 1, 3, 3, 1]
            },
            new() {
                SetId = DriveDiscSetId.ShadowHarmony,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.AnomalyProficiency, Affix.Pen, Affix.CritRate, Affix.Def],
                Levels = [15, 2, 1, 2, 4]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.ElectricDmgBonus, Affix.AtkRatio, Affix.CritDamage, Affix.Def, Affix.CritRate],
                Levels = [15, 2, 2, 3, 1]
            },
            new() {
                SetId = DriveDiscSetId.ShadowHarmony,
                Rarity = Rarity.S,
                Stats = [Affix.ImpactRatio, Affix.CritDamage, Affix.Hp, Affix.AnomalyProficiency, Affix.Atk],
                Levels = [15, 3, 2, 2, 1]
            },
        ],
        Team = [
            AgentId.Soldier0Anby
        ],
        StunBonus = 1.5,
        Rotation = [
            // "silenced_shot 1",
            // "silenced_shot 1",
            // "silenced_shot 1",
            // "silenced_shot 1",
            // "silenced_shot 3",
            $"{AgentId.Soldier0Anby}.sundering_bolt",
            "harmonizing_shot_tartarus 1",
            "harmonizing_shot_tartarus 2",
            $"{AgentId.Soldier0Anby}.azure_flash",
            "harmonizing_shot",
            $"{AgentId.Soldier0Anby}.white_thunder",
            $"{AgentId.Soldier0Anby}.azure_flash",
            "harmonizing_shot",
            $"{AgentId.Soldier0Anby}.white_thunder",
            $"{AgentId.Soldier0Anby}.azure_flash",
            "harmonizing_shot",
            $"{AgentId.Soldier0Anby}.white_thunder",
            $"{AgentId.Soldier0Anby}.thunder_smite"
        ]
    };

    [Test]
    public void TriggerTest() {
        var enemy = new NotoriousDullahan {
            StunMultiplier = Trigger.StunBonus
        };
        var result = Calculator.Calculate(Trigger.AgentId, Trigger.WeaponId, GetDriveDiscs(Trigger),
            Trigger.Team, Trigger.Rotation, enemy);

        Assert.That(result.PerAction, Is.Not.Empty);

        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
    }
}