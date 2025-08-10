using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public class SilverAnbyTests : AgentsTest {
    private CalcRequest SilverAnby { get; } = new() {
        AgentId = 1381,
        WeaponId = 14138,
        Discs = [
            new () {
                SetId = 32900, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.Def, Affix.AnomalyProficiency, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 1, 3, 2, 3]
            },
            new () {
                SetId = 32900,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.DefRatio, Affix.Def, Affix.CritRate, Affix.CritDamage],
                Levels = [15, 1, 3, 1, 4]
            },
            new () {
                SetId = 32900,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AtkRatio, Affix.CritRate, Affix.CritDamage, Affix.Atk],
                Levels = [15, 2, 3, 3, 1]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.CritRate, Affix.CritDamage, Affix.DefRatio, Affix.AtkRatio, Affix.HpRatio],
                Levels = [15, 3, 3, 1, 1]
            },
            new () {
                SetId = 32700,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.HpRatio, Affix.CritRate, Affix.CritDamage, Affix.AnomalyProficiency],
                Levels = [15, 2, 3, 2, 1]
            },
            new () {
                SetId = 32900,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.Hp, Affix.AnomalyProficiency, Affix.DefRatio, Affix.CritDamage],
                Levels = [15, 2, 1, 3, 3]
            },
        ],
        Team = [],
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
            "thunder_smite"
        ]
    };

    [Test]
    public void SilverAnbyTest() {
        var enemy = new NotoriousDullahan {
            StunMultiplier = SilverAnby.StunBonus
        };
        var result = Calculator.Calculate(SilverAnby.AgentId, SilverAnby.WeaponId, GetDriveDiscs(SilverAnby), 
            SilverAnby.Team, SilverAnby.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction);
        Console.WriteLine($"Total: {result.Total}");
    }
}