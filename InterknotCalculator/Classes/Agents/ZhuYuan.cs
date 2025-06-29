using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class ZhuYuan : Agent {
    public ZhuYuan() : base(1241) {
        Speciality = Speciality.Attack;
        Element = Element.Ether;
        Rarity = Rarity.S;
        Faction = Faction.CriminalInvestigationSpecialResponseTeam;

        Stats[Affix.Hp] = 7482;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 919;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.788;
        Stats[Affix.Impact] = 90;
        Stats[Affix.AnomalyMastery] = 93;
        Stats[Affix.AnomalyProficiency] = 92;
        Stats[Affix.DmgBonus] = 0.4;
        Stats[Affix.EnergyRegen] = 1.2;

        Skills["dont_move"] = new(SkillTag.BasicAtk, [
            new(87.1, 32.6, Element: Element.Physical),
            new(252.9, 146.8, 104.77),
            new(274.8, 143.1, Element: Element.Physical),
            new(302.8, 187, 59.12),
            new(325, 209.6, 60.05)
        ]);
        Skills["please_do_not_resist_phys"] = new(SkillTag.BasicAtk, [
            new(107.6, 90.4, Element: Element.Physical),
            new(107.6, 90.4, Element: Element.Physical),
            new(322.6, 268.9, Element: Element.Physical)
        ]);
        Skills["please_do_not_resist"] = new(SkillTag.BasicAtk, [
            new(272.3, 90.4, 54.15),
            new(272.3, 90.4, 54.15),
            new(815.8, 268.9, 162.45)
        ]);
        
        Skills["firepower_offensive"] = new(SkillTag.Dash, [
            new(111.2, 41.9, 50)
        ]);
        Skills["overwhelming_firepower"] = new(SkillTag.Dash, [
            new(107.6, 90.4, Element: Element.Physical),
            new(272.3, 90.4, 54.15)
        ]);
        Skills["fire_blast"] = new(SkillTag.Counter, [
            new(353.9, 242.8, 46.7)
        ]);
        
        Skills["covering_shot"] = new(SkillTag.QuickAssist, [
            new(103.1, 77.8, 46.7)
        ]);
        Skills["defensive_counter"] = new(SkillTag.FollowUpAssist, [
            new(712.2, 463.7, 301.97)
        ]);
        
        Skills["buckshot_blast"] = new(SkillTag.Special, [
            new(37.1, 28.3, 16.65)
        ]);
        Skills["full_barrage"] = new(SkillTag.ExSpecial, [
            new(1174.8, 720.9, 485.12)
        ]);
        
        Skills["eradication_mode"] = new(SkillTag.Chain, [
            new(1174, 223.4, 334.5)
        ]);
        Skills["max_eradication_mode"] = new(SkillTag.Ultimate, [
            new(3955.4, 194, 103.33)
        ]);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality == Speciality.Support) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(0.3, Affix.CritRate)];
        }

        return [];
    }
}