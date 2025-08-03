using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public sealed class JaneDoe : Agent, IAgentReference<JaneDoe> {
    public static JaneDoe Reference() {
        var jane = new JaneDoe {
            Stats = {
                [Affix.Atk] = 2400,
                [Affix.AnomalyProficiency] = 420,
                [Affix.AnomalyMastery] = 195
            }
        };

        var sharpenedStinger = Resources.Current.GetWeapon(14126);
        foreach (var passive in sharpenedStinger.Passive) {
            jane.BonusStats[passive.Affix] += passive.Value;
        }

        jane.ApplyPassive();
        
        return jane;
    }
    
    public JaneDoe() : base(1261) {
        Speciality = Speciality.Anomaly;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.CriminalInvestigationSpecialResponseTeam;

        Stats[Affix.Hp] = 7788;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 880;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 148;
        Stats[Affix.AnomalyProficiency] = 114;

        Skills["dancing_blades"] = new(SkillTag.BasicAtk, [
            new(72.4, 23.0, 21.34, 0.501),
            new(125.0, 67.4, 52.06, 1.448),
            new(167.1, 90.3, 68.78, 1.946),
            new(327.3, 164.7, 127.45, 3.589),
            new(197.8, 103.9, 76.71, 2.246),
            new(582.8, 300.1, 226.62, 6.543)
        ]);
        Skills["salchow_jump"] = new(SkillTag.BasicAtk, [
            new(602.2, 344.7, 273.4, 7.499),
            new(323, 184.5, 146.56, 4.02),
        ]);

        Skills["edge_jump"] = new(SkillTag.Dash, [
            new(143, 54.5, 32.5, 1.17),
            new(143, 54.5, 32.5, 1.17),
        ]);
        Skills["phantom_thrust"] = new(SkillTag.Dash, [
            new(209, 78.7, 47.48, 1.71)
        ]);
        Skills["swift_shadow"] = new(SkillTag.Counter, [
            new(683.3, 344.7, 177.67, 3.9),
            new(683.3, 344.7, 177.67, 3.9)
        ]);
        Skills["swift_shadow_dance"] = new(SkillTag.Counter, [
            new(774.2, 371.8, 194.31, 4.499)
        ]);

        Skills["dark_thorn"] = new(SkillTag.QuickAssist, [
            new(239.1, 179.7, 108.33, 3.9)
        ]);
        Skills["lutz_jump"] = new(SkillTag.QuickAssist, [
            new(275, 206.8, 124.96, 4.499)
        ]);
        Skills["last_defense"] = new(SkillTag.DefensiveAssist, [
            new(0, 407.7),
            new(0, 514.4),
            new(0, 250.4)
        ]);
        Skills["gale_sweep"] = new(SkillTag.FollowUpAssist, [
            new(692, 448.6, 292.92)
        ]);

        Skills["aerial_sweep"] = new(SkillTag.Special, [
            new(116.1, 87.5, 52.5)
        ]);
        Skills["aerial_sweep_cleanout"] = new(SkillTag.ExSpecial, [
            new(1150, 702.4, 473.96, -60)
        ]);

        Skills["flowers_of_sin"] = new(SkillTag.Chain, [
            new(1266.2, 356.4, 439.46)
        ]);
        Skills["final_curtain"] = new(SkillTag.Ultimate, [
            new(2941.3, 280, 966.58)
        ]);
    }

    public override void ApplyPassive() {
        Anomalies[Element.Physical] = Anomaly.GetAnomalyByElement(Element.Physical)! with {
            Bonuses = [
                new(0.4 + AnomalyProficiency * 0.0016, Affix.CritRate),
                new(0.5, Affix.CritDamage)
            ],
            CanCrit = true
        };

        BonusStats[Affix.AnomalyBuildupBonus] += 0.25;
        
        if (AnomalyProficiency > 120) {
            BonusStats[Affix.Atk] += Math.Min((AnomalyProficiency - 120) * 2, 600);
        }
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(0.35, Affix.AnomalyBuildupBonus)];
        }

        return [];   
    }
}