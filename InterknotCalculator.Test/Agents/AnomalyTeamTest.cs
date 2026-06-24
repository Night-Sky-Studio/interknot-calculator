using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

public class AnomalyTeamTest : AgentsTest {
    private CalcRequest Request { get; } = new() {
        AgentId = AgentId.Alice,
        WeaponId = WeaponId.PracticedPerfection,
        Discs = [
            new () {
                SetId = DriveDiscSetId.FangedMetal, 
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.Def, Affix.AtkRatio, Affix.AnomalyProficiency, Affix.CritRate],
                Levels = [15, 3, 3, 2, 1]
            },
            new () {
                SetId = DriveDiscSetId.PhaethonsMelody,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.AtkRatio, Affix.Def, Affix.AnomalyProficiency, Affix.CritRate],
                Levels = [15, 3, 1, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.FangedMetal,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritDamage, Affix.AtkRatio, Affix.AnomalyProficiency, Affix.Atk],
                Levels = [15, 1, 3, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.FangedMetal,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyProficiency, Affix.AtkRatio, Affix.Atk, Affix.HpRatio, Affix.CritRate],
                Levels = [15, 3, 1, 2, 2]
            },
            new () {
                SetId = DriveDiscSetId.FangedMetal,
                Rarity = Rarity.S,
                Stats = [Affix.PhysicalDmgBonus, Affix.CritDamage, Affix.HpRatio, Affix.AnomalyProficiency, Affix.Pen],
                Levels = [15, 1, 2, 2, 3]
            },
            new () {
                SetId = DriveDiscSetId.PhaethonsMelody,
                Rarity = Rarity.S,
                Stats = [Affix.AnomalyMasteryRatio, Affix.AnomalyProficiency, Affix.DefRatio, Affix.AtkRatio, Affix.Def],
                Levels = [15, 1, 2, 2, 3]
            },
        ],
        Team = [
            new(AgentId.Vivian, WeaponId.ElegantVanity),
            new(AgentId.Yuzuha, WeaponId.Metanukimorphosis, DriveDiscSetId.MoonlightLullaby)
        ],
        StunBonus = 1.5,
        Rotation = [
            $"{AgentId.Vivian}.violet_requiem",
            $"{AgentId.Vivian}.fluttering_frock_suspension",
            $"{AgentId.Yuzuha}.cavity_alert",
            "intertwined_stab",
            "starshine_waltz 3",
            "aurora_thrust_northern_cross",
            "aurora_thrust_southern_cross",
            "celestial_overture 6",
            "starshine_waltz 3"
        ]
    };
    
    [Test]
    public void AliceVivianYuzuhaTest() {
        var result = Calculator.Calculate(Request);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Assert.That(result.PerAction, 
            Has.Some.Matches<AgentAction>(a => a.Tag is SkillTag.AttributeAnomaly));
        
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }
}