using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;
using Microsoft.AspNetCore.Connections;

namespace InterknotCalculator.Classes.Agents;

public class SilverAnby : Agent, IAgentReference<SilverAnby> {
    public static SilverAnby Reference() {
        var silverAnby = new SilverAnby();
        silverAnby.ApplyPassive();
        return silverAnby;
    }
    
    public SilverAnby() : base(AgentId.Soldier0Anby) {
        Speciality = Speciality.Attack;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.NewEriduDefenseForce;
        
        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 854 + 75;
        Stats[Affix.CritRate] = 0.05 + 0.144;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 93;
        Stats[Affix.AnomalyMastery] = 94;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;
        
        Skills["penetrating_shock"] = new(SkillTag.BasicAtk, [
            new (65.70, 42.20, 25.32, 0.91),
            new (124.90, 82.00, 49.54, 1.78),
            new (160.40, 108.20, 65.27, 2.35),
            new (309.60, 207.00, 125.16, 4.51),
            new (297.00, 196.90, 118.95, 4.28),
        ]);
        Skills["penetrating_shock_finisher"] = new(SkillTag.BasicAtk, [
            new (80.60, 94.00, 56.40, 2.03)
        ]);

        Skills["torrent"] = new(SkillTag.Dash, [
            new (165.00, 62.20, 37.48, 1.35),
        ]);
        Skills["ground_flash_counter"] = new(SkillTag.Counter, [
            new (501.40, 328.20, 98.30, 3.54),
        ]);

        Skills["cloud_flash"] = new(SkillTag.QuickAssist, [
            new (217.10, 163.20, 98.30, 1.77),
            new (163.20, 163.20, 98.30, 1.77),
        ]);
        Skills["counter_surge"] = new(SkillTag.DefensiveAssist, [
            new (0.00, 407.70),
            new (0.00, 514.40),
            new (0.00, 250.40),
        ]);
        Skills["conducting_blow"] = new(SkillTag.FollowUpAssist, [
            new (815.20, 534.40, 346.92),
            new (534.40, 534.40, 346.92),
        ]);

        Skills["celestial_thunder"] = new(SkillTag.Special, [
            new (120.10, 90.40, 54.16),
        ]);
        Skills["white_thunder"] = new(SkillTag.Aftershock, [
            new (334.40, 0.00)
        ]);
        Skills["azure_flash"] = new(SkillTag.Special, [
            new (87.00, 50.70, 30.03),
        ]);
        Skills["thunder_smite"] = new(SkillTag.Aftershock, [
            new (376.20, 0.00),
        ]);
        Skills["sundering_bolt"] = new(SkillTag.ExSpecial, [
            new (760.20, 688.50, 464.96, -60.00),
        ]);

        Skills["leaping_thunderstrike"] = new(SkillTag.Chain, [
            new (1128.10, 298.20, 379.50),
        ]);
        Skills["voidstrike"] = new(SkillTag.Ultimate, [
            new (3470.70, 415.50, 251.66),
        ]);
    }

    public override void ApplyPassive() {
        BonusStats[Affix.DmgBonus] += 0.25;
        TagBonus.Add(new(0.3 * CritDamage, Affix.CritDamage, [SkillTag.Aftershock]));
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Stun) ||
            team.Any(a => a.Speciality == Speciality.Support)) {
            ExternalTagBonus.Add(new(0.25, Affix.DmgBonus, [SkillTag.Aftershock]));
            return [
                new(0.1, Affix.CritRate)
            ];
        }

        return [];
    }
}