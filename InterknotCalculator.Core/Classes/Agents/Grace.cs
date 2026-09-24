using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Grace : Agent {
    public Grace() : base(AgentId.Grace) {
        Speciality = Speciality.Anomaly;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.BelobogHeavyIndustries;

        InitializeStats(new () {
            [Affix.Hp] = 7482,
            [Affix.Def] = 600,
            [Affix.Atk] = 750 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 83,
            [Affix.AnomalyMastery] = 115 + 36,
            [Affix.AnomalyProficiency] = 116,
            [Affix.EnergyRegen] = 1.2
        });
        
        Skills["high-pressure_spike"] = new(SkillTag.BasicAtk, [
            new(111.2, 27.9, element: Element.Physical, energy: 0.615),
            new(120.2, 52.3, element: Element.Physical, energy: 1.189),
            new(250.2, 107.9, 64.6, 2.454),
            new(373.3, 161.1, element: Element.Physical, energy: 4.081),
            new(81, 61.2, element: Element.Physical, energy: 1.38)
        ]);

        Skills["quick_inspection"] = new(SkillTag.Dash, [
            new(67.4, 25.5, element: Element.Physical, energy: 0.571)
        ]);
        Skills["violation_penalty"] = new(SkillTag.Counter, [
            new(329.2, 226.4, 43.33, 1.56)
        ]);

        Skills["incident_management"] = new(SkillTag.QuickAssist, [
            new(91.7, 68.6, 43.33, 1.56)
        ]);
        Skills["counter_volt_needle"] = new(SkillTag.FollowUpAssist, [
            new(719, 470.1, 320.01)
        ]);

        Skills["obstruction_removal"] = new(SkillTag.Special, [
            new(85, 64.1, 70.03)
        ]);
        Skills["supercharged_obstruction_removal"] = new(SkillTag.ExSpecial, [
            new(334.1 * 2, 201.3 * 2, 143.34 , -40)
        ]);

        Skills["collaborative_construction"] = new(SkillTag.Chain, [
            new(1143.3, 229.3, 344.5)
        ]);
        Skills["demolition_blast"] = new(SkillTag.Ultimate, [
            new(2958.3, 200.2, 895.7)
        ]);
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            AnomalyBuildupBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 1.3, 
                tags: SkillTag.Special | SkillTag.ExSpecial));

            if (c.Team.Values.Any(a => a.Element.Matches(Element) || a.Faction == Faction)) {
                DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.36, 
                    tags: SkillTag.AttributeAnomaly));
            }
        });
    }
}