using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class Yanagi : Agent {
    private bool KagenActive { get; set; }
    
    private void ToggleStance() {
        // Yanagi has two stances: Jougen and Kagen
        // The trick is - these buffs only apply after an ExSpecial attack
        // so, unless it's used - these buffs are not applied
        KagenActive = !KagenActive;
        if (KagenActive) {
            BonusStats[Affix.ElectricDmgBonus] -= 0.1;
            BonusStats[Affix.PenRatio] += 0.1;
        } else {
            BonusStats[Affix.ElectricDmgBonus] += 0.1;
            BonusStats[Affix.PenRatio] -= 0.1;
        }
    }
    
    public Yanagi() : base(1221) {
        Speciality = Speciality.Anomaly;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.HollowSpecialOperationsSection6;
        
        Stats[Affix.Hp] = 7788;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 797 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 112 + 36;
        Stats[Affix.AnomalyProficiency] = 114;
        Stats[Affix.EnergyRegen] = 1.2;
        
        Skills["tsukuyomi_kagura_jougen"] = new(SkillTag.BasicAtk, [
            new(113.8, 42.1, Element: Element.Physical, Energy: 1.009),
            new(199.8, 102.2, Element: Element.Physical, Energy: 2.474),
            new(226.4, 104.6, 79.91, 2.522),
            new(254.2, 117.2, 89.42, 2.822),
            new(474.5, 217.8, 167.35, 5.28),
        ]);
        Skills["tsukuyomi_kagura_kagen"] = new(SkillTag.BasicAtk, [
            new(226.4, 84.1, Element: Element.Physical, Energy: 2.017),
            new(259.0, 140.4, Element: Element.Physical, Energy: 3.385),
            new(146.5, 67.7, 51.38, 1.622),
            new(215.5, 100.2, 75.07, 2.401),
            new(544.6, 250.3, 192.04, 6.059),
        ]);

        Skills["fleeting_flight"] = new(SkillTag.Dash, [
            new(101, 68.5, Element: Element.Physical, Energy: 1.65)
        ]);
        Skills["rapid_retaliation"] = new(SkillTag.Counter, [
            new(463.7, 275.6, 76.52, 3.062)
        ]);

        Skills["blade_of_elegance"] = new(SkillTag.QuickAssist, [
            new(188.2, 127.1, 76.52, 3.062)
        ]);
        Skills["radiant_reversal"] = new(SkillTag.DefensiveAssist, [
            new(0, 366.3),
            new(0, 463.7),
            new(0, 226.1)
        ]);
        Skills["weeping_willow_stab"] = new(SkillTag.FollowUpAssist, [
            new(815.2, 481.2, 312.23)
        ]);

        Skills["ruten"] = new(SkillTag.Special, [
            new(235.1, 159.6, 96)
        ]);
        Skills["gekka_ruten"] = new(SkillTag.ExSpecial, [
            new(327.7, 190.9, 143.5, -20),
            new(756.2, 164.6, 268.54, -20)
        ]);

        Skills["celestial_harmony"] = new(SkillTag.Chain, [
            new(1187.1, 268.5, 359.58)
        ]);
        Skills["raiei_tenge"] = new(SkillTag.Ultimate, [
            new(3024.3, 147, 904.39)
        ]);
    }

    private bool IsPolarityDisorder { get; set; }
    protected override double GetDisorderBaseMultiplier(Element element, double attack, Func<double, double>? mvReducer = null) {
        return IsPolarityDisorder
            ? GetDisorderTimeMultiplier(element, prev => prev + 2.5) * attack * 0.15 + 32 * AnomalyProficiency
            : base.GetDisorderBaseMultiplier(element, attack, prev => prev + 2.5);
    }
    
    public override IEnumerable<AgentAction> GetActionDamage(string skill, int scale, Enemy enemy) {
        var result = base.GetActionDamage(skill, scale, enemy).ToList();

        switch (Skills[skill].Tag) {
            case SkillTag.ExSpecial:
            case SkillTag.FollowUpAssist:
                ToggleStance();
                if (scale == 1) {
                    goto case SkillTag.Ultimate;
                }
                break;
            case SkillTag.Ultimate: {
                if (enemy.AfflictedAnomaly is not null) {
                    IsPolarityDisorder = true;
                    result.Add(GetAnomalyDamage(Element.None, enemy) with {
                        AgentId = Id,
                        Name = "polarity_disorder"
                    });
                    IsPolarityDisorder = false;
                }
                break;
            }
        }
        
        return result;
    }

    public override void ApplyPassive() {
        BonusStats[Affix.ElectricDmgBonus] += 0.2;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality) ||
            team.Any(a => a.Element == Element)) {
            return [new(0.45, Affix.AnomalyBuildupBonus)];
        }

        return [];
    }
}