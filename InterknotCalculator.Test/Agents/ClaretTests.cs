using System.Collections.Immutable;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class ClaretTests : AgentsTest {
    protected override CalcRequest Request { get; } = new() {
        AgentId = AgentId.Claret,
        WeaponId = WeaponId.CrimsonThirst,
        Discs = [
            new() {
                SetId = DriveDiscSetId.ThornedRose,
                Rarity = Rarity.S,
                Stats = [Affix.Hp, Affix.CritRate, Affix.DefRatio, Affix.Def, Affix.Pen],
                Levels = [15, 3, 2, 1, 1]
            },
            new() {
                SetId = DriveDiscSetId.ThornedRose,
                Rarity = Rarity.S,
                Stats = [Affix.Atk, Affix.CritRate, Affix.DefRatio, Affix.Def, Affix.Hp],
                Levels = [15, 2, 3, 1, 1]
            },
            new() {
                SetId = DriveDiscSetId.ThornedRose,
                Rarity = Rarity.S,
                Stats = [Affix.Def, Affix.CritRate, Affix.DefRatio, Affix.Pen, Affix.AtkRatio],
                Levels = [15, 3, 2, 1, 1]
            },
            new() {
                SetId = DriveDiscSetId.ThornedRose,
                Rarity = Rarity.S,
                Stats = [Affix.CritRate, Affix.DefRatio, Affix.Def, Affix.Pen, Affix.Hp],
                Levels = [15, 3, 2, 1, 1]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.ElectricDmgBonus, Affix.CritRate, Affix.DefRatio, Affix.Def, Affix.AtkRatio],
                Levels = [15, 2, 2, 2, 1]
            },
            new() {
                SetId = DriveDiscSetId.WoodpeckerElectro,
                Rarity = Rarity.S,
                Stats = [Affix.DefRatio, Affix.CritRate, Affix.Def, Affix.Pen, Affix.Hp],
                Levels = [15, 3, 2, 1, 1]
            }
        ],
        Team = [],
        Enemy = new(nameof(NotoriousDullahan), 1.5),
        Rotation = [
            "bloodbloom_oath_secret_forging_techniques",
            
            "bloodbloom_oath_starforging 1",
            "bloodbloom_oath_starforging 2",
            "bloodbloom_oath_starforging 3",
            "bloodbloom_oath_starforging 1",
            "bloodbloom_oath_starforging 2",
            "bloodbloom_oath_starforging 3",
            "bloodbloom_oath_starforging 1",
            "bloodbloom_oath_starforging 2",
            
            "bloodbloom_oath_blood_burial_assault"
        ]
    };

    [Test]
    public async Task ClaretTest() {
        var result = Calculator.Calculate(Request);

        Assert.That(result.PerAction, Is.Not.Empty);
        Assert.That(result.PerAction, Has.Exactly(3)
            .Matches<AgentAction>(a => a.Name == "maim"));

        AssertRotationOrderPreserved(Request.Rotation, result.PerAction.ToImmutableList(), Request.AgentId);

        await VerifyActions(result.PerAction);
        
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nGash: {result.Enemy.GashBuildup} ({result.Enemy.GashStacks} stacks)");
        Console.WriteLine($"Enemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
    }

    private const string EnterCrimsonInscription = "bloodbloom_oath_secret_forging_techniques";
    
    [Test]
    public void CritConversionTest() {
        var claret = new Claret();
        var stats = claret.CollectStats(true);
        
        // CRIT DMG conversion: 0.338 + 0.5 * 0.35
        Assert.That(stats[Affix.CritRate], Is.EqualTo(0.05 + 0.288 + 0.5 * 0.35).Within(1e-9));
    }
    
    private double GetFinalCritRate(params string[] rotation) {
        var result = Calculator.Calculate(Request with {
            Team = [], Rotation = rotation
        });

        return result.FinalStats.CalculatedStats[Affix.CritRate];
    }


    [Test]
    public void CrimsonInscriptionStatsTest() {
        var outside = GetFinalCritRate("bloodforging_four_forms 1");
        var inside = GetFinalCritRate(EnterCrimsonInscription);

        // In the state, the bonus shows in the combat stats
        Assert.That(inside - outside, Is.EqualTo(0.3).Within(1e-9));
    }

    [Test]
    public void CrimsonInscriptionDurationTest() {
        var outside = GetFinalCritRate("bloodforging_four_forms 1");
        string[] sixteen = [EnterCrimsonInscription, ..Enumerable.Repeat("bloodbloom_oath_starforging 1", 16)];

        // 16 actions are in the state, and the state ends after the 16th
        var result = GetFinalCritRate(sixteen);
        Assert.That(result, Is.EqualTo(outside).Within(1e-9));

        // The 17th Crimson Inscription ability is impossible
        Assert.Throws<InvalidOperationException>(() => GetFinalCritRate([..sixteen, "bloodbloom_oath_starforging 1"]));
    }

    [Test]
    public void CrimsonInscriptionChainUltimateDoNotCountTest() {
        string[] rotation = [
            EnterCrimsonInscription,
            ..Enumerable.Repeat("bloodbloom_oath_starforging 1", 15),
            "bloodbloom_oath_resonant_blood_pact",
            "bloodbloom_oath_trial_after_trial",
            // 16th counted action
            "bloodbloom_oath_starforging 1"
        ];

        Assert.DoesNotThrow(() => GetFinalCritRate(rotation));
        Assert.Throws<InvalidOperationException>(() => GetFinalCritRate([..rotation, "bloodbloom_oath_starforging 1"]));
    }

    [Test]
    public void CrimsonInscriptionGuardTest() {
        Assert.Throws<InvalidOperationException>(() => GetFinalCritRate("bloodbloom_oath_starforging 1"));
        Assert.DoesNotThrow(() => GetFinalCritRate(EnterCrimsonInscription, "bloodbloom_oath_starforging 1"));
        // Chain Attack and Ultimate do not need the state
        Assert.DoesNotThrow(() => GetFinalCritRate("bloodbloom_oath_resonant_blood_pact", "bloodbloom_oath_trial_after_trial"));

        // In the state, the inputs give the Crimson Inscription versions
        Assert.Throws<InvalidOperationException>(() => GetFinalCritRate(EnterCrimsonInscription, EnterCrimsonInscription));
        Assert.Throws<InvalidOperationException>(() => GetFinalCritRate(EnterCrimsonInscription, "bloodforging_four_forms 1"));
        Assert.Throws<InvalidOperationException>(() => GetFinalCritRate(EnterCrimsonInscription, "porcelumes_first_call"));
        // Abilities that both states share
        Assert.DoesNotThrow(() => GetFinalCritRate(EnterCrimsonInscription, "unyielding_heart_of_the_furnace 1",
            "bloodbloom_oath_pureforged_edge", "give_not_an_inch_of_steel", "bloodbloom_oath_hammer_into_shape"));
        // After the state ends, the normal abilities are available again
        Assert.DoesNotThrow(() => GetFinalCritRate([
            EnterCrimsonInscription, ..Enumerable.Repeat("bloodbloom_oath_starforging 1", 16),
            "bloodforging_four_forms 1", EnterCrimsonInscription
        ]));
    }

    [Test]
    public void CrimsonInscriptionNoDoubleCountTest() {
        // A non-Armorer W-Engine: the Crimson Thirst bonus after the EX Special would change the damage
        var request = Request with { WeaponId = WeaponId.CloudcleaveRadiance, Team = [] };
        const string chain = "bloodbloom_oath_resonant_blood_pact";

        var outside = Calculator.Calculate(request with { Rotation = [chain] })
            .PerAction.Single(a => a.Name == chain);
        var inside = Calculator.Calculate(request with { Rotation = [EnterCrimsonInscription, chain] })
            .PerAction.Single(a => a.Name == chain);

        // Outside the state, the tag filter gives the bonus. In the state, the stance does. Never both.
        Assert.That(inside.Damage, Is.EqualTo(outside.Damage).Within(1e-6));
    }

    [Test]
    public void GashGaugeTest() {
        var claret = new Claret();
        var ctx = new Context {
            Team = { [claret.Id] = claret },
            MainAgentId = claret.Id
        };
        var enemy = ctx.Enemy;

        enemy.AddGashBuildup(ctx, claret, 599);
        Assert.That(enemy.GashStacks, Is.Zero);
        Assert.That(enemy.TryConsumeGash(), Is.False);

        enemy.AddGashBuildup(ctx, claret, 1300);
        Assert.That(enemy.GashStacks, Is.EqualTo(3));
        Assert.That(enemy.GashBuildup, Is.EqualTo(1800));

        enemy.AddGashBuildup(ctx, claret, 100);
        enemy.AddGashStack();
        Assert.That(enemy.GashBuildup, Is.EqualTo(1800));

        Assert.That(enemy.TryConsumeGash(), Is.True);
        Assert.That(enemy.TryConsumeGash(), Is.True);
        Assert.That(enemy.TryConsumeGash(), Is.True);
        Assert.That(enemy.TryConsumeGash(), Is.False);
    }
}
