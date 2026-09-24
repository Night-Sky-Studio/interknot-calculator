using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Yanagi : Agent, IPolarityDisorderAgent {
    private bool KagenActive { get; set; }

    private static ModifierKey JougenKey { get; } = new("Jougen");
    private static ModifierKey KagenKey { get; } = new("Kagen");
    
    private void ToggleStance() {
        // Yanagi has two stances: Jougen and Kagen
        // The trick is - these buffs only apply after an ExSpecial attack
        // so, unless it's used - these buffs are not applied
        KagenActive = !KagenActive;
        if (KagenActive) {
            ElementalDmgBonus.RemoveKey(ModifierKey.Agent(Id) + ModifierKey.CorePassive() + JougenKey);
            PenRatio.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive() + KagenKey, 0.1));
        } else {
            PenRatio.RemoveKey(ModifierKey.Agent(Id) + ModifierKey.CorePassive() + KagenKey);
            ElementalDmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive() + JougenKey, 0.1));
        }
    }
    
    public Yanagi() : base(AgentId.Yanagi) {
        Speciality = Speciality.Anomaly;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.HollowSpecialOperationsSection6;
        
        InitializeStats(new () {
            [Affix.Hp] = 7788,
            [Affix.Def] = 612,
            [Affix.Atk] = 797 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 86,
            [Affix.AnomalyMastery] = 112 + 36,
            [Affix.AnomalyProficiency] = 114,
            [Affix.EnergyRegen] = 1.2
        });
        
        Skills["tsukuyomi_kagura_jougen"] = new(SkillTag.BasicAtk, [
            new(113.8, 42.1, element: Element.Physical, energy: 1.009),
            new(199.8, 102.2, element: Element.Physical, energy: 2.474),
            new(226.4, 104.6, 79.91, 2.522),
            new(254.2, 117.2, 89.42, 2.822),
            new(474.5, 217.8, 167.35, 5.28),
        ]);
        Skills["tsukuyomi_kagura_kagen"] = new(SkillTag.BasicAtk, [
            new(226.4, 84.1, element: Element.Physical, energy: 2.017),
            new(259.0, 140.4, element: Element.Physical, energy: 3.385),
            new(146.5, 67.7, 51.38, 1.622),
            new(215.5, 100.2, 75.07, 2.401),
            new(544.6, 250.3, 192.04, 6.059),
        ]);

        Skills["fleeting_flight"] = new(SkillTag.Dash, [
            new(101, 68.5, element: Element.Physical, energy: 1.65)
        ]);
        Skills["rapid_retaliation"] = new(SkillTag.Counter, [
            new(463.7, 275.6, 76.52, 3.062)
        ]);

        Skills["blade_of_elegance"] = new(SkillTag.QuickAssist, [
            new(188.2, 127.1, 76.52, 3.062)
        ]);
        Skills["radiant_reversal"] = new(SkillTag.DefensiveAssist, [
            new(0, 366.3),
            new(0, 463.7),
            new(0, 226.1)
        ]);
        Skills["weeping_willow_stab"] = new(SkillTag.FollowUpAssist, [
            new(815.2, 481.2, 312.23)
        ]);

        Skills["ruten"] = new(SkillTag.Special, [
            new(235.1, 159.6, 96)
        ]);
        Skills["gekka_ruten"] = new(SkillTag.ExSpecial, [
            new(327.7, 190.9, 143.5, -20),
            new(756.2, 164.6, 268.54, -20)
        ]);

        Skills["celestial_harmony"] = new(SkillTag.Chain, [
            new(1187.1, 268.5, 359.58)
        ]);
        Skills["raiei_tenge"] = new(SkillTag.Ultimate, [
            new(3024.3, 147, 904.39)
        ]);
    }

    // TODO: check if this flag can be replaced with event bus
    private bool IsPolarityDisorder { get; set; }
    protected override double GetDisorderBaseMultiplier(Element element, double attack, Func<double, double>? mvReducer = null) {
        return IsPolarityDisorder
            ? GetDisorderTimeMultiplier(element, prev => prev + 2.5) * attack * 0.15 + 32 * AnomalyProficiency
            : base.GetDisorderBaseMultiplier(element, attack, prev => prev + 2.5);
    }

    public AgentAction GetPolarityDisorder(Context ctx) {
        IsPolarityDisorder = true;
        var polarityDisorder = GetAnomalyDamage(ctx, Element.None) with {
            AgentId = Id,
            Name = "polarity_disorder"
        };
        IsPolarityDisorder = false;
        return polarityDisorder;
    }
    
    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnCalculationStarted.Add(c => {
            ElementalDmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.2));

            if (c.Team.Values.Any(a => a.Speciality == Speciality || a.Element.Matches(Element))) {
                AnomalyBuildupBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.45));
            }
        });
        
        ctx.Events.OnActionExecuted.Add((c, e) => {
            if (e.Agent != this) return;
            if (e.Ability.Tag is not (SkillTag.Ultimate or SkillTag.ExSpecial)) return;
            ToggleStance();
            // only trigger PD on downward attack, which is only the second hit of Gekka Ruten
            if (e.Ability.Tag is SkillTag.ExSpecial && e.Ability.Scale != 1) return;
            if (c.Enemy.AfflictedAnomaly is null) return;

            ctx.ActionsQueue.Add(GetPolarityDisorder(ctx));
        });
    }
}