using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class Vivian : Agent {
    public Vivian() : base(1331) {
        Speciality = Speciality.Anomaly;
        Element = Element.Ether;
        Rarity = Rarity.S;
        Faction = Faction.Mockingbird;
        
        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 805 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 108 + 36;
        Stats[Affix.AnomalyProficiency] = 118;
        Stats[Affix.EnergyRegen] = 1.2;

        Skills["feathered_strike"] = new(SkillTag.BasicAtk, [
            new (68.20, 64.40, Element: Element.Physical, Energy: 1.39),
            new (48.40, 42.40, Element: Element.Physical, Energy: 0.92),
            new (153.90, 123.60, Element: Element.Physical, Energy: 2.67),
            new (257.10, 191.00, 86.61, 4.16),
        ]);
        Skills["noblewoman_waltz"] = new(SkillTag.BasicAtk, [
            new (96.20, 55.90, 33.75, 1.22),
        ]);
        Skills["fluttering_frock_suspension"] = new(SkillTag.BasicAtk, [
            new (160.50, 94.30, 56.65, 2.04),
        ]);
        Skills["featherbloom"] = new(SkillTag.BasicAtk, [
            new (440.00, 0.00, 126.00),
        ]);

        Skills["silver_thorned_melody"] = new(SkillTag.Dash, [
            new (100.20, 38.00, Element: Element.Physical, Energy: 0.81),
        ]);
        Skills["wingblade_reverb"] = new(SkillTag.Counter, [
            new (472.90, 311.80, 70.69, 3.18),
        ]);

        Skills["frostwing_guard"] = new(SkillTag.QuickAssist, [
            new (195.20, 74.00, 35.34, 1.59),
            new (74.00, 74.00, 35.34, 1.59),
        ]);
        Skills["silver_umbrella_formation"] = new(SkillTag.DefensiveAssist, [
            new (0.00, 407.70),
            new (0.00, 514.40),
            new (0.00, 250.40),
        ]);
        Skills["featherblade_execution"] = new(SkillTag.FollowUpAssist, [
            new (747.90, 488.10, 236.56),
            new (488.10, 488.10, 236.56),
        ]);

        Skills["song_of_silver_wings"] = new(SkillTag.Special, [
            new (114.10, 85.50, 41.34),
        ]);
        Skills["violet_requiem"] = new(SkillTag.ExSpecial, [
            new (1185.50, 727.90, 352.56, -60.00),
        ]);

        Skills["chorus_of_celestial_wings"] = new(SkillTag.Chain, [
            new (1317.80, 330.00, 319.57),
        ]);
        Skills["soaring_birds_song"] = new(SkillTag.Ultimate, [
            new (3374.20, 374.80, 808.56),
        ]);
    }
    
    public double GetAbloomDamage(Agent agent, Enemy enemy) {
        return 0.0d;
    }
    
    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        return base.ApplyTeamPassive(team);
    }
}