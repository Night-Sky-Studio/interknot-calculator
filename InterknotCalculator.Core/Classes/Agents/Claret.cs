using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public class Claret : ArmorerAgent {
    /// <summary>
    /// Abilities that exist only in the Crimson Inscription state
    /// </summary>
    private static readonly HashSet<string> CrimsonInscriptionAbilities = [
        "bloodbloom_oath_starforging",
        "bloodbloom_oath_subduing_axe",
        "bloodbloom_oath_cleaving_gold_and_iron",
        "bloodbloom_oath_blood_burial_assault",
        "bloodbloom_oath_returning_edge",
        "bloodbloom_oath_return_of_the_dire_moon"
    ];

    /// <summary>
    /// Abilities that exist only outside the Crimson Inscription state
    /// </summary>
    private static readonly HashSet<string> OutsideCrimsonInscriptionAbilities = [
        "bloodforging_four_forms",
        "tempering",
        "returning_edge",
        "porcelumes_first_call",
        "return_of_the_dire_moon",
        "bloodbloom_oath_secret_forging_techniques"
    ];

    /// <summary>
    /// Since Crimson Inscription state lasts 16s and calculator has no timing mechanism yet, we assume that
    /// this state lasts for 16 actions after EX special. In-game, I was able to use ~12-16 abilities on a test dummy
    /// Chain and Ultimate do not count - the state's duration does not decrease during them
    /// </summary>
    private const int CrimsonInscriptionDuration = 16;

    private int CrimsonInscriptionActionsLeft { get; set; }
    private bool IsInCrimsonInscription => CrimsonInscriptionActionsLeft > 0;

    private const SkillTag CrimsonInscriptionTags =
        SkillTag.Chain | SkillTag.Ultimate | SkillTag.CounterAssist | SkillTag.FollowUpAssist;

    public Claret() : base(AgentId.Claret) {
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.Roscaelifer;

        InitializeStats(new() {
            [Affix.Hp] = 5652,
            [Affix.Def] = 441,
            [Affix.Atk] = 626,
            [Affix.CritRate] = 0.05 + 0.288,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 93,
            [Affix.AnomalyMastery] = 80,
            [Affix.AnomalyProficiency] = 79,
            [Affix.LacerationDmg] = 1.5
        });

        Skills["bloodforging_four_forms"] = new(SkillTag.BasicAtk, [
            new(109.4, 52.3, 31.51, GashBuildup: 31.51),
            new(135.4, 64.9, 39, GashBuildup: 39),
            new(190.9, 91.7, 55.3, GashBuildup: 55.3),
            new(579.9, 269, 162.54, GashBuildup: 162.54),
        ]);
        Skills["bloodbloom_oath_starforging"] = new(SkillTag.BasicAtk, [
            new(215.5, 82.5, 49.97, GashBuildup: 59.96),
            new(575.6, 220.4, 133.34, GashBuildup: 160),
            new(904.2, 280.5, 170, GashBuildup: 204),
            new(355.5, 110.8, 66.67, GashBuildup: 80),
        ]);
        Skills["bloodbloom_oath_subduing_axe"] = new(SkillTag.BasicAtk, [new(656.9, 203.9, 123.34, GashBuildup: 148.01)]);
        Skills["tempering"] = new(SkillTag.Dash, [new(106.8, 65.1, 39.16, GashBuildup: 39.16)]);
        Skills["returning_edge"] = new(SkillTag.Counter, [new(545.3, 295.2, 228.34, GashBuildup: 78.34)]);
        Skills["bloodbloom_oath_returning_edge"] = new(SkillTag.Counter, [new(660, 286.4, 223.34, GashBuildup: 88.01)]);
        Skills["return_of_the_dire_moon"] = new(SkillTag.QuickAssist, [new(106.8, 65.1, 39.17, GashBuildup: 39.17)]);
        Skills["bloodbloom_oath_return_of_the_dire_moon"] = new(SkillTag.QuickAssist, [
            new(232.4, 61.3, 73.34, GashBuildup: 36.67)
        ]);
        Skills["unyielding_heart_of_the_furnace"] = new(SkillTag.DefensiveAssist, [
            new(0, 407.7),
            new(0, 514.4),
            new(0, 250.4),
        ]);
        Skills["give_not_an_inch_of_steel"] = new(SkillTag.CounterAssist, [new(2306.9, 1072.4, 671.67, GashBuildup: 446)]);
        Skills["bloodbloom_oath_pureforged_edge"] = new(SkillTag.FollowUpAssist, [
            new(892.8, 474.2, 156.67, GashBuildup: 156.67)
        ]);
        // When the heavy attack hits, adds 1 Gash stack directly
        Skills["bloodbloom_oath_hammer_into_shape"] = new(SkillTag.FollowUpAssist, [
            new(1713.2, 790.4, 411.67, GashBuildup: 134)
        ]);
        Skills["porcelumes_first_call"] = new(SkillTag.Special, [new(180.3, 55.4, 33.35, GashBuildup: 33.35)]);
        // Scale 2 (the horizontal slash) triggers Maim
        Skills["bloodbloom_oath_cleaving_gold_and_iron"] = new(SkillTag.Special, [
            new(153.6, 47.7, 56.71),
            new(175.7, 54.5, 64.94),
        ]);
        // 15 hits in the animation, holding the skill triggers up to 3 Maims
        Skills["bloodbloom_oath_blood_burial_assault"] = new(SkillTag.Special, [new(626.3, 382.5, 231.71)]);
        Skills["bloodbloom_oath_secret_forging_techniques"] = new(SkillTag.ExSpecial, [
            new(1249.6, 388.3, 234.97, GashBuildup: 281.97)
        ]);
        Skills["bloodbloom_oath_resonant_blood_pact"] = new(SkillTag.Chain, [new(2116.2, 415.5, 251.67, GashBuildup: 302)]);
        Skills["bloodbloom_oath_trial_after_trial"] = new(SkillTag.Ultimate, [new(4504.8, 530.9, 821.64, GashBuildup: 400.97)]);

        // Maim DMG, triggered by consuming 1 Gash stack
        // TODO: verify on Sunbringer if it should remain in Skills
        Skills["maim"] = new(SkillTag.Special, [new(1625.6, 0)]);
    }

    /// <summary>
    /// For every 1% of Claret's initial CRIT DMG, her initial CRIT Rate increases by 0.35%
    /// </summary>
    public double CritConversion => CritDamage.InitialValue * 0.35;

    public override SafeDictionary<Affix, double> CollectStats(bool initial = false) {
        var result = base.CollectStats(initial);
        result[Affix.CritRate] += CritConversion;
        return result;
    }

    protected override double GetCritMultiplier(in Hit hit) =>
        GetLacerationMultiplier(CritRate.For(hit.Ability) + CritConversion, LacerationDmg.For(hit.Ability));

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);

        var corePassiveKey = ModifierKey.Agent(Id) + ModifierKey.CorePassive();
        var teamPassiveKey = ModifierKey.Agent(Id) + ModifierKey.TeamPassive();
        List<Modifier> remnantEdge = [];

        ctx.Events.OnCalculationStarted += c => {
            // While Claret is in the Crimson Inscription state, or while using a Chain Attack, Ultimate,
            // Counter Assist, or Assist Follow-Up, CRIT Rate increases by 30% and Gash Buildup Rate
            // increases by 50%
            CritRate.Add(corePassiveKey, 0.3, ModifierType.CombatFlat, 
                when: () => IsInCrimsonInscription);
            GashBuildupBonus.Add(corePassiveKey, 0.5, ModifierType.CombatFlat, 
                when: () => IsInCrimsonInscription);
            CritRate.Add(corePassiveKey, 0.3, ModifierType.CombatFlat, CrimsonInscriptionTags,
                when: () => !IsInCrimsonInscription);
            GashBuildupBonus.Add(corePassiveKey, 0.5, ModifierType.CombatFlat, CrimsonInscriptionTags,
                when: () => !IsInCrimsonInscription);

            // When another character in your squad is a Stun or Armorer character, or shares the same attribute:
            // When Claret or another squad member triggers Maim, all Armorer Agents in the squad enter
            // Remnant Edge. While in the Remnant Edge state, Laceration DMG is increased by 25%, lasting 40s
            if (c.HasTeammates(a => a.Speciality is Speciality.Stun or Speciality.Armorer || a.Element == Element, Id)) {
                foreach (var agent in c.Team.Values) {
                    if (agent.Speciality is Speciality.Armorer) {
                        remnantEdge.Add(agent.GetStat(Affix.LacerationDmg)
                            .Add(teamPassiveKey, 0.25, ModifierType.CombatFlat, active: false));
                    }
                }
            }
        };

        ctx.Events.OnMaim += (_, _) => remnantEdge.ForEach(m => m.Active = true);
    }

    public override AgentAction GetActionDamage(Context ctx, Ability ability) {
        if (!IsInCrimsonInscription && CrimsonInscriptionAbilities.Contains(ability.Name))
            throw new InvalidOperationException($"Cannot use {ability.Name} outside of Crimson Inscription");
        if (IsInCrimsonInscription && OutsideCrimsonInscriptionAbilities.Contains(ability.Name))
            throw new InvalidOperationException($"Cannot use {ability.Name} in Crimson Inscription " +
                                                $"({CrimsonInscriptionActionsLeft} actions left)");

        var action = base.GetActionDamage(ctx, ability);

        // Each hit against an enemy with Gash consumes 1 stack to trigger Maim
        var maimCount = ability switch {
            { Name: "bloodbloom_oath_cleaving_gold_and_iron", Scale: 1 } => 1,
            { Name: "bloodbloom_oath_blood_burial_assault" } => 3,
            _ => 0
        };
        for (var i = 0; i < maimCount && ctx.Enemy.TryConsumeGash(); i++) {
            ctx.ActionsQueue.Add(TriggerMaim(ctx));
        }

        if (ability.Name is "bloodbloom_oath_hammer_into_shape") {
            ctx.Enemy.AddGashStack();
        }
        
        if (ability.Name is "bloodbloom_oath_secret_forging_techniques") {
            CrimsonInscriptionActionsLeft = CrimsonInscriptionDuration;
        } else if (IsInCrimsonInscription && ability.Name is not "maim"
                                          && ability.Tag is not (SkillTag.Chain or SkillTag.Ultimate)) {
            CrimsonInscriptionActionsLeft--;
        }

        return action;
    }
}