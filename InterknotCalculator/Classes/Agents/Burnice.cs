using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public class Burnice : Agent, IAgentReference<Burnice> {
    public static Burnice Reference() {
        var burnice = new Burnice {
            Stats = {
                [Affix.Atk] = 2850, 
                [Affix.AnomalyMastery] = 135.5, 
                [Affix.AnomalyProficiency] = 375,
                [Affix.FireDmgBonus] = 0.3,
            }
        };
        
        burnice.ApplyPassive();

        return burnice;
    }

    public Burnice() : base(1171) {
        Speciality = Speciality.Anomaly;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.SonsOfCalydon;

        Stats[Affix.Hp] = 7368;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 863 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 83;
        Stats[Affix.AnomalyMastery] = 118;
        Stats[Affix.AnomalyProficiency] = 120;
        Stats[Affix.EnergyRegen] = 1.2 + 0.36;

        Skills["direct_flame_blend"] = new(SkillTag.BasicAtk, [
            new(89.9, 28.6, 16.94, 0.61),
            new(87.5, 46.2, Element: Element.Physical, Energy: 1.007),
            new(144.3, 72, 43.37, 1.562),
            new(133.7, 45.8, 27.54, 0.992),
            new(193.1, 78.4, 47.19, 1.699)
        ]);
        Skills["mixed_flame_blend"] = new(SkillTag.BasicAtk, [
            new(250.8, 144.9, 95.98, 1.578),
            new(466, 26.3, 171.11, 2.931)
        ]);

        Skills["dangerous_fermentation"] = new(SkillTag.Dash, [
            new(136.1, 51.6, 30.83, 1.11)
        ]);
        Skills["fluttering_steps"] = new(SkillTag.Counter, [
            new(439.7, 292.3, 76.66, 1.38)
        ]);

        Skills["energizing_speciality_drink"] = new(SkillTag.QuickAssist, [
            new(169.1, 127.3,76.66, 1.38)
        ]);
        Skills["smoky_cauldron"] = new(SkillTag.DefensiveAssist, [
            new(0, 407.7),
            new(0, 514.4),
            new(0, 250.4)
        ]);
        Skills["scorching_dew"] = new(SkillTag.FollowUpAssist, [
            new(655.4, 424.3, 277.22)
        ]);

        Skills["intense_heat_aging_method"] = new(SkillTag.Special, [
            new(116.1, 87.5, 52.51),
            new(125.1, 94.3, 56.68)
        ]);
        Skills["intense_heat_stirring_method"] = new(SkillTag.ExSpecial, [
            new(1088.3, 568.9, 411.1, -12.5),
            new(193.5, 98.7, 71.55, -5)
        ]);
        Skills["intense_heat_stirring_method_double"] = new(SkillTag.ExSpecial, [
            new(1916.2, 879.4, 530.24, -25),
            new(574.2, 312.3, 183.65, -10)
        ]);

        Skills["fuel-fed_flame"] = new(SkillTag.Chain, [
            new(1261.8, 363, 1002.96)
        ]);
        Skills["glorious_inferno"] = new(SkillTag.Ultimate, [
            new(4025.2, 160.3, 861.16)
        ]);
    }


    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(0.65, Affix.AnomalyBuildupBonus)];
        }

        return [];
    }
}