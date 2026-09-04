using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class YeShunguangTests : AgentsTest {
    private static string[] Combo { get; } = [
        "enlightened_mind_sunderlight 1",
        "enlightened_mind_sunderlight 2",
        "enlightened_mind_sunderlight_annihilation 2",
        "enlightened_mind_sunderlight_maximum",
        "enlightened_mind_soaring_light",
        "enlightened_mind_skyward_ascent",
    ];
    
    private static string[] EntryRotation { get; } = [
        $"{AgentId.Sunna}.special_photography_technique",
        $"{AgentId.Zhao}.burst_of_frost",
            
        // enter racist mode
        "illuminating_darkness",
            
        // combo 1
        "#enlightened_mind_combo",
            
        // combo 2
        "#enlightened_mind_combo",
            
        // finisher
        "enlightened_mind_return_to_dust"
    ];
    
    public static string[] SixComboRotation { get; } = [
        $"{AgentId.Sunna}.special_photography_technique",
        
        $"{AgentId.Dialyn}.rock",
        $"{AgentId.Dialyn}.scissors",
        $"{AgentId.Dialyn}.paper",
        
        // give her ult
        $"{AgentId.Dialyn}.get_lost",
        "chasing_storms",
        
        // combo 1
        "#enlightened_mind_combo",
        // combo 2
        "#enlightened_mind_combo",
        
        // finisher
        "enlightened_mind_return_to_dust",
        
        $"{AgentId.Sunna}.special_photography_technique",
        
        // enter racist mode
        "illuminating_darkness",
            
        // combo 1
        "#enlightened_mind_combo",
        // combo 2
        "#enlightened_mind_combo",
        
        // finisher
        "enlightened_mind_return_to_dust",
        
        // ult
        "chasing_storms",
            
        // combo 1
        "#enlightened_mind_combo",
        // combo 2
        "#enlightened_mind_combo",
        
        // finisher
        "cleaving_heavens"
    ];
    
    protected override CalcRequest Request { get; } = new() {
        AgentId = AgentId.YeShunguang,
        WeaponId = WeaponId.CloudcleaveRadiance,
        Discs = [
            new() {
                SetId = DriveDiscSetId.BranchBladeSong,
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritDamage, Affix.DefRatio, Affix.CritRate, Affix.Def],
                Levels = [15, 1, 2, 4, 1]
            },
            new() {
                SetId = DriveDiscSetId.WhiteWaterBallad,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.CritDamage, Affix.AnomalyProficiency, Affix.DefRatio],
                Levels = [15, 1, 3, 1, 3]
            },
            new() {
                SetId = DriveDiscSetId.WhiteWaterBallad,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.AtkRatio, Affix.CritDamage, Affix.Atk, Affix.CritRate],
                Levels = [15, 1, 3, 2, 2]
            },
            new() {
                SetId = DriveDiscSetId.WhiteWaterBallad,
                Rarity = Rarity.S,
                Stats = [Affix.CritDamage, Affix.CritRate, Affix.Def, Affix.Atk, Affix.Pen],
                Levels = [15, 3, 2, 1, 3]
            },
            new() {
                SetId = DriveDiscSetId.WhiteWaterBallad,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.CritDamage, Affix.Pen, Affix.CritRate, Affix.Hp],
                Levels = [15, 2, 1, 2, 3]
            },
            new() {
                SetId = DriveDiscSetId.BranchBladeSong,
                Rarity = Rarity.S,
                Stats = [Affix.AtkRatio, Affix.Hp, Affix.CritDamage, Affix.Atk, Affix.CritRate],
                Levels = [15, 2, 2, 3, 1]
            }
        ],
        Team = [],
        StunBonus = 1,
        Rotation = []
    };
    
    [Test]
    public async Task YeShunguangEntryRotationTest() {
        var request = Request with {
            Team = [
                new(AgentId.Zhao, WeaponId.HalfSugarBunny, DriveDiscSetId.BunnyInWonderland),
                new(AgentId.Sunna, WeaponId.Thoughtbop, DriveDiscSetId.MoonlightLullaby)
            ],
            Rotation = EntryRotation
        };
        var result = Calculator.Calculate(request);
        
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
    
    [Test]
    public async Task YeShunguangSixComboRotationTest() {
        var request = Request with {
            Team = [
                new(AgentId.Dialyn, WeaponId.YesterdayCalls, DriveDiscSetId.KingOfTheSummit),
                new(AgentId.Sunna, WeaponId.Thoughtbop, DriveDiscSetId.MoonlightLullaby)
            ],
            Rotation = SixComboRotation
        };
        var result = Calculator.Calculate(request);
        
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