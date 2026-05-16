using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public class Alice : Agent {
    public Alice() : base(AgentId.Alice) {
        Speciality = Speciality.Anomaly;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.SpookShack;

        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 880;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 142;
        Stats[Affix.AnomalyProficiency] = 118;
        Stats[Affix.EnergyRegen] = 1.2;

        Skills["celestial_overture"] = new(SkillTag.BasicAtk, [
            new(111.2, 60.6, 29.76, 1.441),
            new(176, 105, 52.13, 2.538),
            new(133.2, 90.7, 44.45, 2.179),
            new(224.1, 157.9, 77.81, 3.82),
            new(261.2, 197.7, 97.2, 4.787),
            new(437.2, 316.5, 156, 7.667),
        ]);
        Skills["starshine_waltz"] = new(SkillTag.BasicAtk, [
            new(378.1, 83.9, 78.16),
            new(609.1, 117.2, 109.66),
            new(1931.5, 296.2, 467.83),
        ]);
        
        Skills["blade_dancers_gale"] = new(SkillTag.Dash, [
            new(151.2, 51.5, 23.92, 1.231)
        ]);
        Skills["ceremony_of_swordlight"] = new(SkillTag.Counter, [
            new(568.9, 329.5, 85.16, 4.38)
        ]);
        Skills["intertwined_stab"] = new(SkillTag.QuickAssist, [
            new(327.2, 220.6, 103.85, 2.671)
        ]);
        Skills["parry_guard"] = new(SkillTag.DefensiveAssist, [
            new(0, 366.3),
            new(0, 463.7),
            new(0, 226.1),
        ]);
        
        Skills["cross_riposte"] = new(SkillTag.FollowUpAssist, [
            new(666, 388.2, 197.17)
        ]);
        Skills["piercing_dawn"] = new(SkillTag.Special, [
            new(125.1, 84.7, 39.65)
        ]);
        Skills["aurora_thrust_northern_cross"] = new(SkillTag.ExSpecial, [
            new(920.9, 517.9, 317.06, -40)
        ]);
        Skills["aurora_thrust_southern_cross"] = new(SkillTag.ExSpecial, [
            new(1064.4, 608.8, 364.46, -40)
        ]);
        Skills["starfall_intermission"] = new(SkillTag.Chain, [
            new(1332.9, 308.1, 284.34)
        ]);
        Skills["starfall_finale"] = new(SkillTag.Ultimate, [
            new(4524.7, 364.7, 722.25)
        ]);
    }
    
    public override void ApplyPassive() {
        BonusStats[Affix.AnomalyProficiency] += Math.Max(AnomalyMastery - 140, 0) * 1.6;
        base.ApplyPassive();
    }

    private bool BuildupBonusActive { get; set; } = false;
    
    public override void RegisterHooks(Context ctx) {
        // Core passive
        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            if (e.Element is not (Element.Physical or Element.HonedEdge)) return;
            
            var action = e.Agent.GetAnomalyDamage(c, e.Element, true);
            action.Damage *= 0.175;
            action.Name = "twin_rainbows_of_the_swordheart";
            c.Actions.Add(action);
        });

        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            if (e.Agent != this && e.Element is Element.Physical) return;
            if (BuildupBonusActive) return;
            
            BuildupBonusActive = true;
            BonusStats[Affix.AnomalyBuildupBonus] += 0.125;
        });
        
        // Hold E for polarity assault
        ctx.Events.OnActionExecuted.Add((c, e) => {
            if (e.Agent != this || e.Ability is not { Name: "starshine_waltz", Scale: 2 }) return;
            var polarityAssault = GetAnomalyDamage(c, Element.Physical, true) with {
                Name = "polarity_assault"
            };
            c.ActionsQueue.Add(polarityAssault);

            if (c.Enemy.AfflictedAnomaly is null) return;
            var disorder = GetAnomalyDamage(c, Element.None, true);
            c.Enemy.AfflictedAnomaly = null;
            c.ActionsQueue.Add(disorder);
        });
    }

    public override AgentAction GetAnomalyDamage(Context ctx, Element element, bool skipEvents = false) {
        if (element is Element.None && ctx.Enemy.AfflictedAnomaly?.Element is Element.Physical) {
            var bonus = Math.Min(0.18 * 7, 1.8);
            BonusStats[Affix.DisorderDmgBonus] += bonus;
            try {
                return base.GetAnomalyDamage(ctx, element, skipEvents);
            } finally {
                BonusStats[Affix.DisorderDmgBonus] -= bonus;
            }
        }
        
        return base.GetAnomalyDamage(ctx, element, skipEvents);
    }
}