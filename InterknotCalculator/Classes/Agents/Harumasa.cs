using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public class Harumasa : Agent {
    public Harumasa() {
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

        Skills["cloud_piercer"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(85.3, 40.8, Element: Element.Physical),
                new(80.5, 67.4, Element: Element.Physical),
                new(142.4, 100.2),
                new(180.3, 134.7),
                new(266, 173.7)
            ]
        };
        Skills["cloud_piercer_drift"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [new(139.9, 74.2, Element: Element.Physical)]
        };
        Skills["falling_feather"] = new() {
              Tag = SkillTag.BasicAtk,
              Scales = [new(211, 79.1)]
        };
        Skills["ha-oto_no_ya"] = new() { // That'd be a fucking pain to simulate...
            Tag = SkillTag.BasicAtk,
            Scales = [new(32.4, 0)]
        };

        Skills["hiten_no_tsuru"] = new() {
            Tag = SkillTag.Dash, 
            Scales = [new(162.1, 61.3, Element: Element.Physical)]
        };
        Skills["hidden_edge"] = new() {
            Tag = SkillTag.Dash,
            Scales = [new(439.6, 292.2)]
        };
        Skills["hiten_no_tsuru_slash"] = new() {
            Tag = SkillTag.Dash,
            Scales = [
                new(325.1, 99),
                new(333.8, 69),
                new(379.9, 71.2)
            ],
            Affixes = {
                [Affix.CritRate] = 0.25,
                [Affix.CritDamage] = 0.36
            }
        };

        Skills["braced_bow"] = new() {
            Tag = SkillTag.QuickAssist, 
            Scales = [new(169, 127.2)]
        };
        Skills["yugamae"] = new() {
            Tag = SkillTag.DefensiveAssist,
            Scales = [
                new(0, 371.9),
                new(0, 443.7),
                new(0, 179.7)
            ]
        };
        Skills["yugamae_slash"] = new() {
            Tag = SkillTag.FollowUpAssist,
            Scales = [new(615.1, 395.3)]
        };

        Skills["nowhere_to_hide"] = new() {
            Tag = SkillTag.Special,
            Scales = [new(105.1, 78.7)]
        };
        Skills["nowhere_to_run"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [new(899.2, 674.5)]
        };

        Skills["kai_hanare"] = new() {
            Tag = SkillTag.Chain,
            Scales = [new(1035.7, 275.8)]
        };        
        Skills["zanshin"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [new(3908.6, 160.8)]
        };
    }
    
    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Stun) ||
            team.Any(a => a.Speciality == Speciality.Anomaly)) {
            return [new(0.4, Affix.DmgBonus)];
        }
        
        return [];
    }
}