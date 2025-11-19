using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Yixuan : RuptureAgent, ICustomAnomaly {
    public Element AnomalyElement { get; set; }
    public Yixuan() : base(AgentId.Yixuan) {
        Element = Element.Ether;
        AnomalyElement = Element.AuricInk;
        Rarity = Rarity.S;
        Faction = Faction.YunkuiSummit;

        Stats[Affix.Hp] = 8373;
        Stats[Affix.Atk] = 872;
        Stats[Affix.Def] = 441;
        Stats[Affix.CritRate] = 0.194;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 93;
        Stats[Affix.AnomalyMastery] = 92;
        Stats[Affix.AnomalyProficiency] = 90;

        Anomalies[Element.AuricInk] = new(62.5 * 20, Element.Ether, []);

        Skills["cirrus_strike"] = new(SkillTag.BasicAtk, [
            new(92.00, 42.90, 25.96),
            new(131.50, 104.10, 62.60),
            new(175.10, 128.70, 77.91),
            new(210.40, 167.90, 101.59),
            new(228.10, 159.60, 96.06),
        ]);
        Skills["ink_veil_cloud_coalescence"] = new(SkillTag.BasicAtk, [
            new(470.10, 440.80, 266.70),
        ]);
        Skills["auric_array"] = new(SkillTag.BasicAtk, [
            new(611.00, 440.80, 266.70),
        ]);
        Skills["qingming_eruption"] = new(SkillTag.BasicAtk, [
            new(221.70, 160.20, 96.63),
        ]);

        Skills["skybreaker"] = new(SkillTag.Dash, [
            new(100.50, 47.70, 28.33),
        ]);
        Skills["banishing_blow"] = new(SkillTag.Counter, [
            new(439.60, 355.30, 114.96),
        ]);

        Skills["cloudstream_shadow"] = new(SkillTag.QuickAssist, [
            new(202.40, 95.20, 57.48),
            new(95.20, 95.20, 57.48),
        ]);
        Skills["clear_sky_surge"] = new(SkillTag.DefensiveAssist, [
            new(0.00, 407.70),
            new(0.00, 514.40),
            new(0.00, 250.40),
        ]);
        Skills["celestial_cloud_blitz"] = new(SkillTag.FollowUpAssist, [
            new(594.00, 484.60, 315.51),
            new(484.60, 484.60, 315.51),
        ]);

        Skills["shadow_ignition"] = new(SkillTag.Special, [
            new(104.90, 98.10, 59.16),
        ]);
        Skills["ink_manifestation"] = new(SkillTag.ExSpecial, [
            new(600.60, 259.80, 197.87, -40.00),
            new(741.30, 441.80, 299.04, -40.00),
            new(853.20, 428.30, 305.45, -40.00),
        ]);
        Skills["ink_manifestation_charged"] = new(SkillTag.ExSpecial, [
            new(600.60 + 299.20, 259.80 + 114.30, 197.87 + 55.83),
        ]);
        Skills["cloud_shaper"] = new(SkillTag.ExSpecial, [
            new(1343.90, 720.90, 503.33, -40.00),
        ]);
        Skills["ashen_ink_becomes_shadows"] = new(SkillTag.ExSpecial, [
            new(468.60, 213.60, 157.16, -40.00),
        ]);

        Skills["auric_ink_rush"] = new(SkillTag.Chain, [
            new(1066.60, 341.80, 406.20),
        ]);
        Skills["qingming_skyshade"] = new(SkillTag.Ultimate, [
            new(3706.90, 611.70, 370.03),
        ]);
        Skills["endless_talisman_suppression"] = new(SkillTag.Ultimate, [
            new(2932.50, 374.80, 226.70),
        ]);
    }

    public override Stat? ApplyAbilityPassive(string ability) => Skills[ability].Tag switch {
        SkillTag.ExSpecial or 
        SkillTag.FollowUpAssist or 
        SkillTag.Chain or 
        SkillTag.Ultimate => new(Affix.DmgBonus, 0.6),
        _ => ability is "auric_array" or "qingming_eruption" ? new(Affix.DmgBonus, 0.6) : null
    };

    private bool IsTeamPassiveActive { get; set; } = false;

    public override IEnumerable<AgentAction> GetActionDamage(string skill, int scale, Enemy enemy) {
        if (IsTeamPassiveActive && enemy.StunMultiplier > 1.0 
                                && skill is "cloud_shaper" or "ashen_ink_becomes_shadows") {
            Skills[skill].Affixes[Affix.DmgBonus] = 0.3;
        }

        return base.GetActionDamage(skill, scale, enemy);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality is Speciality.Stun or Speciality.Defense or Speciality.Support 
                          || a.Faction == Faction)) {
            IsTeamPassiveActive = true;
            return [
                new (Affix.CritDamage, 0.4)
            ];
        }

        return [];
    }
}