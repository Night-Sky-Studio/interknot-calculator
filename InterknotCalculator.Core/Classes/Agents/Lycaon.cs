using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Lycaon : Agent, IStunAgent, IAgentReference<Lycaon> {
    public static Lycaon Reference() {
        var lycaon = new Lycaon();

        lycaon.ApplyPassive();
        
        return lycaon;
    }

    public Lycaon() : base(AgentId.Lycaon) {
        Speciality = Speciality.Stun;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.VictoriaHousekeeping;
        
        Stats[Affix.Hp] = 8416;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 653 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 119 + 18;
        Stats[Affix.AnomalyMastery] = 90;
        Stats[Affix.AnomalyProficiency] = 91;
        Stats[Affix.EnergyRegen] = 1.2;
        
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

    public override void ApplyPassive() {
        BonusStats[Affix.DazeBonus] = 0.8;
        ExternalBonus[Affix.IceResPen] = 0.25;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];
        
        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            EnemyStunBonusOverride = 0.35;
        }

        return [];
    }

    public double EnemyStunBonusOverride { get; set; }
}