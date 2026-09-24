using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Lycaon : SupportAgent, IAgentReference<Lycaon> {
    public static Lycaon Reference(uint weaponId, uint setId) {
        var lycaon = new Lycaon();
        
        lycaon.InitializeStats(new () {
            [Affix.CritRate] = 0.5
        });

        lycaon.SetWeaponPassive(weaponId);
        lycaon.SetDriveDiscsPassive(setId);
        
        return lycaon;
    }

    public Lycaon() : base(AgentId.Lycaon) {
        Speciality = Speciality.Stun;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.VictoriaHousekeeping;
        
        InitializeStats(new () {
            [Affix.Hp] = 8416,
            [Affix.Def] = 606,
            [Affix.Atk] = 653 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 119 + 18,
            [Affix.AnomalyMastery] = 90,
            [Affix.AnomalyProficiency] = 91,
            [Affix.EnergyRegen] = 1.2
        });
        
        Skills["moon_hunter"] = new(SkillTag.BasicAtk, [
            new (58.90, 22.30, element: Element.Physical, energy: 0.50),
            new (74.50, 18.70, 11.48, 0.41),
            new (70.10, 45.70, element: Element.Physical, energy: 1.04),
            new (113.60, 49.40, 31.28, 1.13),
            new (117.80, 68.70, element: Element.Physical, energy: 1.56),
            new (199.60, 81.20, 51.13, 1.84),
            new (304.90, 168.10, element: Element.Physical, energy: 3.84),
            new (422.10, 161.00, 101.98, 3.67),
            new (362.20, 222.50, element: Element.Physical, energy: 5.06),
            new (555.90, 245.60, 155.24, 5.59),
            new (712.10, 309.00, 195.75, 7.05),
        ]);

        Skills["keep_it_clean"] = new(SkillTag.Dash, [
            new (94.60, 35.80, element: Element.Physical, energy: 0.81),
        ]);
        Skills["etiquette_manual"] = new(SkillTag.Counter, [
            new (374.00, 252.80, 60.03, 2.16),
        ]);

        Skills["wolf_pack"] = new(SkillTag.QuickAssist, [
            new (126.90, 95.00, 60.03, 2.16),
            new (95.00, 95.00, 60.03, 2.16),
        ]);
        Skills["disrupted_hunt"] = new(SkillTag.DefensiveAssist, [
            new (0.00, 388.80),
            new (0.00, 491.20),
            new (0.00, 239.60),
        ]);
        Skills["vengeful_counterattack"] = new(SkillTag.FollowUpAssist, [
            new (577.60, 371.10, 254.76),
            new (371.10, 371.10, 254.76),
        ]);

        Skills["time_to_hunt"] = new(SkillTag.Special, [
            new (109.3 + 45.90, 81.8 + 34.90, 25.83 + 21.66),
            new (221.4 + 45.90, 166.4 + 34.90, 52.5 + 21.66),
        ]);
        Skills["thrill_of_the_hunt"] = new(SkillTag.ExSpecial, [
            new (564.6 + 505.40, 358.3 + 318.20, 225.42 + 200.76, -40.00),
            new (1075 + 505.40, 679.5 + 318.20, 428.48 + 200.76, -60.00),
        ]);

        Skills["as_you_wish"] = new(SkillTag.Chain, [
            new (1275.80, 328.80, 407.86),
        ]);
        Skills["mission_complete"] = new(SkillTag.Ultimate, [
            new (3389.20, 1645.50, 173.36),
        ]);
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            DazeBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.8));

            foreach (var agent in c.Team.Values) {
                // WORKAROUND:
                //      Agents don't expose convenient "IceResPen" property.
                //      Since it won't have any effect on non-Ice agents, we can
                //      skip adding it for now.
                if (agent.Element.Matches(Element.Ice)) {
                    agent.ElementalResPen.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.25));
                }
            }

            if (c.Team.Values.Any(a => a.Element.Matches(Element) || a.Faction == Faction)) {
                c.Enemy.StunMultiplier.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.35));
            }
        });
    }
}