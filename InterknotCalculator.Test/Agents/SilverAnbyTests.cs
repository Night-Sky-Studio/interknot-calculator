using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

public class SilverAnbyTests : AgentsTest {
    private CalcRequest SilverAnby { get; } = new() {
        AgentId = AgentId.Soldier0Anby,
        WeaponId = WeaponId.SeveredInnocence,
        Discs = [
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritDamage, Affix.Atk, Affix.CritRate, Affix.AtkRatio],
                Levels = [15, 2, 1, 4, 2]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AtkRatio, Affix.CritDamage, Affix.HpRatio, Affix.CritRate],
                Levels = [15, 3, 2, 1, 3]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.AnomalyProficiency, Affix.AtkRatio, Affix.CritRate],
                Levels = [15, 2, 1, 3, 3]
            },
            new() {
                SetId = DriveDiscSetId.AstralVoice,
                Rarity = Rarity.S,
                Stats = [Affix.CritRate, Affix.Def, Affix.AtkRatio, Affix.CritDamage, Affix.Atk],
                Levels = [15, 1, 2, 4, 2]
            },
            new() {
                SetId = DriveDiscSetId.AstralVoice,
                Rarity = Rarity.S,
                Stats = [Affix.ElectricDmgBonus, Affix.AtkRatio, Affix.CritDamage, Affix.Atk, Affix.CritRate],
                Levels = [15, 2, 3, 1, 3]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.CritRate, Affix.CritDamage, Affix.HpRatio, Affix.Def],
                Levels = [15, 3, 2, 1, 2]
            }
        ],
        // Discs = [
        //     new () {
        //         SetId = DriveDiscSetId.ShadowHarmony, 
        //         Rarity = Rarity.S,
        //         Stats = [Affix.Hp, Affix.AtkRatio, Affix.CritDamage, Affix.CritRate, Affix.Pen],
        //         Levels = [15, 2, 4, 1, 2]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.ShadowHarmony,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.Atk, Affix.CritRate, Affix.Pen, Affix.CritDamage, Affix.AtkRatio],
        //         Levels = [15, 3, 1, 2, 3]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.ShadowHarmony,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.Def, Affix.AtkRatio, Affix.CritRate, Affix.CritDamage, Affix.Atk],
        //         Levels = [15, 2, 3, 3, 1]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.BranchBladeSong,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.CritRate, Affix.CritDamage, Affix.AtkRatio, Affix.Atk, Affix.Pen],
        //         Levels = [15, 3, 2, 1, 3]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.ShadowHarmony,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.ElectricDmgBonus, Affix.CritDamage, Affix.CritRate, Affix.Def, Affix.AtkRatio],
        //         Levels = [15, 1, 3, 1, 3]
        //     },
        //     new () {
        //         SetId = DriveDiscSetId.BranchBladeSong,
        //         Rarity = Rarity.S,
        //         Stats = [Affix.AtkRatio, Affix.Hp, Affix.DefRatio, Affix.CritDamage, Affix.HpRatio],
        //         Levels = [15, 2, 2, 3, 1]
        //     },
        // ],
        Team = [new(AgentId.Trigger, WeaponId.SpectralGaze, DriveDiscSetId.KingOfTheSummit)],
        StunBonus = 1.5,
        Rotation = [
            "leaping_thunderstrike",
            "azure_flash",
            "white_thunder",
            "azure_flash",
            "white_thunder",
            "azure_flash",
            "white_thunder",
            "thunder_smite",
            
            "voidstrike",
            "azure_flash",
            "white_thunder",
            "azure_flash",
            "white_thunder",
            "azure_flash",
            "white_thunder",
            "thunder_smite",
            
            "celestial_thunder",
            "sundering_bolt",
            
            "azure_flash",
            "white_thunder",
            "azure_flash",
            "white_thunder",
            "azure_flash",
            "white_thunder",
            "thunder_smite",
        ]
    };

    [Test]
    public void SilverAnbyTest() {
        var result = Calculator.Calculate(SilverAnby);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        Assert.That(result.PerAction, Has.Exactly(12).Matches<AgentAction>(a => a.Tag is SkillTag.Aftershock));
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
    }
}