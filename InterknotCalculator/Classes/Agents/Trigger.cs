using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public class Trigger : Agent, IStunAgent, IAgentReference<Trigger> {
    public double EnemyStunBonusOverride { get; set; }
    
    public static Trigger Reference() {
        var trigger = new Trigger();
        
        var kingOfTheSummit = Resources.Current.GetDriveDiscSet(DriveDiscSetId.KingOfTheSummit);
        
        foreach (var fullBonus in kingOfTheSummit.FullBonus) {
            trigger.ExternalBonus[fullBonus.Affix] = fullBonus.Value;
        }
        
        trigger.ApplyPassive();
        
        return trigger;
    }
    
    public Trigger() : base(AgentId.Trigger) {
        Speciality = Speciality.Stun;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.NewEriduDefenseForce;
        
        Stats[Affix.Hp] = 7923;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 675 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 113 + 18;
        Stats[Affix.AnomalyMastery] = 96;
        Stats[Affix.AnomalyProficiency] = 95;
        Stats[Affix.EnergyRegen] = 1.2;
        
        Skills["cold_bore_shot"] = new(SkillTag.BasicAtk, [
            new (69.70, 26.10, Element: Element.Physical, Energy: 0.99),
            new (114.10, 42.80, Element: Element.Physical, Energy: 2.00),
            new (222.00, 91.60, Element: Element.Physical, Energy: 3.28),
            new (486.10, 266.00, 174.41, 6.98),
        ]);
        Skills["silenced_shot"] = new(SkillTag.BasicAtk, [
            new (96.00, 35.90, 27.00, 1.36),    // Shot
            new (468.20, 223.60, 193.22, 6.96), // Counter
            new (263.20, 98.60, 119.26, 4.49),  // Finisher
        ]);
        Skills["harmonizing_shot"] = new(SkillTag.Aftershock, [
            new (96.30 * 2, 54.60 * 2, 10.86 * 2)
        ]);
        Skills["harmonizing_shot_tartarus"] = new(SkillTag.Aftershock, [
            new (45.70 * 3, 25.80 * 3, 20.49 * 3),
            new (91.40, 51.60, 40.98),
        ]);

        Skills["vengeful_specter"] = new(SkillTag.Dash, [
            new (103.10, 38.90, Element: Element.Physical, Energy: 0.84),
        ]);
        Skills["condemned_soul"] = new(SkillTag.Counter, [
            new (439.70, 292.30, 76.66, 2.76),
        ]);

        Skills["cold_bore_cover_fire"] = new(SkillTag.QuickAssist, [
            new (169.10, 127.30, 76.66, 2.76),
            new (127.30, 127.30, 76.66, 2.76),
        ]);
        Skills["delaying_demise"] = new(SkillTag.DefensiveAssist, [
            new (0.00, 407.70),
            new (0.00, 514.40),
            new (0.00, 250.40),
        ]);
        Skills["piercing_thunder"] = new(SkillTag.FollowUpAssist, [
            new (866.30, 491.30, 369.51),
            new (491.30, 491.30, 369.51),
        ]);

        Skills["spectral_flash"] = new(SkillTag.Special, [
            new (143.00, 107.80, 65.00),
        ]);
        Skills["flash_burial"] = new(SkillTag.ExSpecial, [
            new (1269.10, 649.40, 356.66, -60.00),
        ]);

        Skills["stygian_guide"] = new(SkillTag.Chain, [
            new (1150.00, 204.00, 322.86),
        ]);
        Skills["underworld_requiem"] = new(SkillTag.Ultimate, [
            new (2961.10, 1384.00, 76.66),
        ]);
    }
    
    public override void ApplyPassive() {
        EnemyStunBonusOverride = 0.35;
    }

    public override IEnumerable<AgentAction> GetActionDamage(string skill, int scale, Enemy enemy) {
        var result = base.GetActionDamage(skill, scale, enemy).ToList();

        result[0].Name = skill switch {
            "silenced_shot" => scale switch {
                1 => "silenced_shot_(counter)",  
                2 => "silenced_shot_(finisher)",
                _ => result[0].Name
            },
            "harmonizing_shot" => "harmonizing_shot_(x2)",
            "harmonizing_shot_tartarus" => scale switch {
                0 => "harmonizing_shot_tartarus_(x3)",
                1 => "harmonizing_shot_tartarus_(finisher)",
                _ => result[0].Name
            },
            _ => result[0].Name
        };

        return result;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Attack) ||
            team.Any(a => a.Element == Element)) {
            if (CritRate > 0.4) {
                return [
                    new(Affix.DazeBonus, Math.Min((CritRate - 0.4) * 0.015, 0.75), tags: [SkillTag.Aftershock])
                ];
            }
        }

        return [];
    }
}