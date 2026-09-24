using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Soldier11 : Agent {
    public Soldier11() : base(AgentId.Soldier11) {
        Speciality = Speciality.Attack;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.NewEriduDefenseForce;

        InitializeStats(new () {
            [Affix.Hp] = 7673,
            [Affix.Def] = 612,
            [Affix.Atk] = 813 + 75,
            [Affix.CritRate] = 0.05 + 0.144,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 93,
            [Affix.AnomalyMastery] = 94,
            [Affix.AnomalyProficiency] = 93,
            [Affix.EnergyRegen] = 1.2
        });

        Skills["warming_sparks"] = new(SkillTag.BasicAtk, [
            new(69.6, 26, element: Element.Physical),
            new(83, 52, element: Element.Physical),
            new(206.2, 124.1, element: Element.Physical),
            new(426.8, 252.3, element: Element.Physical)
        ]);
        Skills["fire_suppression_basic"] = new(SkillTag.BasicAtk, [
            new(111.2, 27.9, 17.05),
            new(114.4, 51.2, 31.91),
            new(264, 113.7, 71.57),
            new(681.7, 287.7, 182.8)
        ], new() {
            [Affix.DmgBonus] = 0.7 // Heatwave passive
        });

        Skills["blazing_fire"] = new(SkillTag.Dash, [
            new(137.6, 51.8, element: Element.Physical)
        ]);
        Skills["fire_suppression_dash"] = new(SkillTag.Dash, [
            new(158, 118.4, 75)
        ], new() {
            [Affix.DmgBonus] = 0.7 // Heatwave passive
        });
        Skills["backdraft"] = new(SkillTag.Counter, [
            new(524.9, 339.1, 114.99)
        ]);

        Skills["covering_fire"] = new(SkillTag.QuickAssist, [
            new(241.8, 181.3, 114.99)
        ]);
        Skills["hold_the_line"] = new(SkillTag.DefensiveAssist, [
            new(0, 388.8),
            new(0, 491.2),
            new(0, 239.6)
        ]);
        Skills["reignition"] = new(SkillTag.FollowUpAssist, [
            new(767.6, 503.8, 342.42)
        ]);

        Skills["raging_fire"] = new(SkillTag.Special, [
            new(105.4, 79, 50.01)
        ]);
        Skills["fervent_fire"] = new(SkillTag.ExSpecial, [
            new(1350.4, 816.3, 580.18)
        ]);

        Skills["uplifting_flame"] = new(SkillTag.Chain, [
            new(1265, 321.4, 402.86)
        ]);
        Skills["bellowing_flame"] = new(SkillTag.Ultimate, [
            new(4206.2, 428, 246.7)
        ]);
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            if (c.Team.Values.Any(a => a.Element.Matches(Element) || a.Faction == Faction)) {
                ElementalDmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.325));
            }
        });
    }
}