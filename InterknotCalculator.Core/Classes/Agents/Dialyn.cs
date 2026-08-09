using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Dialyn : SupportAgent, IStunAgent, IAgentReference<Dialyn> {
    public static Dialyn Reference(uint weaponId, uint setId) {
        var dialyn = new Dialyn {
            Stats = {
                [Affix.CritRate] = 1
            }
        };
        
        dialyn.SetWeapon(weaponId);
        dialyn.SetDriveDiscsPassive(setId);

        return dialyn;
    }

    public Dialyn() : base(AgentId.Dialyn) {
        Speciality = Speciality.Stun;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.KrampusComplianceAuthority;
        
        Stats[Affix.Hp] = 8250;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 758;
        Stats[Affix.CritRate] = 0.194;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 110;
        Stats[Affix.AnomalyMastery] = 94;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;
        
        Skills["happy_to_be_of_service"] = new(SkillTag.BasicAtk, [
            new(52, 29.3, 22.64, 0.408),
            new(103, 65.5, 49.59, 0.893),
            new(129.7, 92.4, 68.72, 1.237),
            new(199.6, 150.1, 125.7, 2.263),
        ]);
        Skills["rock_paper_scissors"] = new(SkillTag.BasicAtk, [
            new(179.7, 107.7, 55, 1.98),
            new(228.3, 137, 70, 2.52),
            new(206.5, 124, 63.34, 2.28),
            new(201.5, 120.7, 61.67, 2.22),
        ]);
        Skills["sudden_call"] = new(SkillTag.Dash, [new(121, 45.7, 55, 0.99)]);
        Skills["number_unavailable"] = new(SkillTag.Counter, [new(534.6, 346.5, 259.97, 3.959)]);
        Skills["forward_call"] = new(SkillTag.QuickAssist, [new(242, 91.3, 109.97, 1.98)]);
        Skills["decline_call"] = new(SkillTag.DefensiveAssist, [
            new(0, 407.7),
            new(0, 514.4),
            new(0, 250.4),
        ]);
        Skills["back_to_back_calls"] = new(SkillTag.FollowUpAssist, [new(753.7, 491.6, 165)]);
        Skills["welcome_gesture"] = new(SkillTag.Special, [new(109.2, 81.7, 49.19)]);
        Skills["get_lost"] = new(SkillTag.ExSpecial, [new(1099.7, 504.8, 160.04, 2.881)]);
        Skills["rock"] = new(SkillTag.ExSpecial, [new(808.8, 354.8, 61.7, -25)]);
        Skills["scissors"] = new(SkillTag.ExSpecial, [new(1050.7, 465.3, 86.67, -25)]);
        Skills["paper"] = new(SkillTag.ExSpecial, [new(1403.5, 641.4, 138.34, -25)]);
        Skills["welcome_mat"] = new(SkillTag.Chain, [new(1240.8, 272.8, 164.97)]);
        Skills["service_stopped_for_number_dialed"] = new(SkillTag.Ultimate, [new(3245, 1151.7, 680)]);

    }

    public double EnemyStunBonusOverride { get; set; } = 0.3;

    public override void ApplyPassive() {
        // If her initial CRIT Rate surpasses 50%, her Impact increases
        // by 2 for each additional 1%, up to a maximum increase of 100.
        BonusStats[Affix.Impact] += Math.Min(100, Math.Max(0, CritRate - 0.5) * 2);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        // When another character in your squad is an Attack or Rupture character
        if (team.Any(a => a is { Speciality: Speciality.Attack or Speciality.Rupture })) {
            // Dialyn's EX Special Attack CRIT DMG is increased by 50%.
            TagBonus.Add(new(Affix.CritDamage, 0.5, tags: [SkillTag.ExSpecial]));

            // When an EX Special Attack or Ultimate is activated, all squad members gain the
            // Overwhelmingly Positive effect.
            // While Overwhelmingly Positive is active, DMG dealt is increased by 40% for 15s.
            ExternalBonus[Affix.DmgBonus] += 0.4;
        }
        
        return base.ApplyTeamPassive(team);
    }
}