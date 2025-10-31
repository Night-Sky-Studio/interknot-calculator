using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public class Harumasa : Agent {
    public Harumasa() : base(1201) {
        Speciality = Speciality.Attack;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.HollowSpecialOperationsSection6;

        Stats[Affix.Hp] = 7405;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 840 + 75;
        Stats[Affix.CritRate] = 0.05 + 0.144;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 90;
        Stats[Affix.AnomalyMastery] = 80;
        Stats[Affix.AnomalyProficiency] = 95;
        Stats[Affix.EnergyRegen] = 1.2;

        Skills["cloud_piercer"] = new(SkillTag.BasicAtk, [
            new(85.3, 40.8, Element: Element.Physical),
            new(80.5, 67.4, Element: Element.Physical),
            new(142.4, 100.2, Element: Element.Physical),
            new(180.3, 134.7, 49.17),
            new(266, 173.7, 65.84)
        ]);
        Skills["cloud_piercer_drift"] = new(SkillTag.BasicAtk, [
            new(139.9, 74.2, Element: Element.Physical)
        ]);
        Skills["falling_feather"] = new(SkillTag.BasicAtk, [
            new(211, 79.1, 33.51)
        ]);
        // That'd be a fucking pain to simulate...
        Skills["ha-oto_no_ya"] = new(SkillTag.BasicAtk, [
            new(32.4, 0)
        ]);

        Skills["hiten_no_tsuru"] = new(SkillTag.Dash, [
            new(162.1, 61.3, Element: Element.Physical)
        ]);
        Skills["hidden_edge"] = new(SkillTag.Dash, [
            new(439.6, 292.2, 53.64)
        ]);
        Skills["hiten_no_tsuru_slash"] = new(SkillTag.Dash, [
            new(325.1, 99, 41.97),
            new(333.8, 69, 29.16),
            new(379.9, 71.2, 31.47)
        ], new() {
            [Affix.CritRate] = 0.25, 
            [Affix.CritDamage] = 0.36
        });

        Skills["braced_bow"] = new(SkillTag.QuickAssist, [
            new(169, 127.2, 53.64)
        ]);
        Skills["yugamae"] = new(SkillTag.DefensiveAssist, [
            new(0, 371.9),
            new(0, 443.7),
            new(0, 179.7)
        ]);
        Skills["yugamae_slash"] = new(SkillTag.FollowUpAssist, [
            new(615.1, 395.3, 181.45)
        ]);

        Skills["nowhere_to_hide"] = new(SkillTag.Special, [
            new(105.1, 78.7, 33.26)
        ]);
        Skills["nowhere_to_run"] = new(SkillTag.ExSpecial, [
            new(899.2, 674.5, 319.11)
        ]);

        Skills["kai_hanare"] = new(SkillTag.Chain, [
            new(1035.7, 275.8, 256.33)
        ]);
        Skills["zanshin"] = new(SkillTag.Ultimate, [
            new(3908.6, 160.8, 61.81)
        ]);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Stun) ||
            team.Any(a => a.Speciality == Speciality.Anomaly)) {
            return [new(Affix.DmgBonus, 0.4)];
        }

        return [];
    }
}