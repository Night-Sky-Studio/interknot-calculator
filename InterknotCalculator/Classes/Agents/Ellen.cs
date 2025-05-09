﻿using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class Ellen : Agent {
    public Ellen() {
        Speciality = Speciality.Attack;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.VictoriaHousekeeping;

        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 863 + 75;
        Stats[Affix.CritRate] = 0.05 + 0.144;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 93;
        Stats[Affix.AnomalyMastery] = 94;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;

        Skills["saw_teeth_trimming"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(98.3, 37.6, Element.Physical),
                new(222.2, 130.8, Element.Physical),
                new(478.6, 306.8, Element.Physical),
            ]
        };
        Skills["flash_freeze_trimming"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(199.7, 74.1),
                new(368.8, 135.3),
                new(993.4, 368.7),
            ]
        };

        Skills["arctic_ambush"] = new() {
            Tag = SkillTag.Dash,
            Scales = [
                new(125, 94.2),
                new(266.8, 152.2),
                new(316.6, 188.9)
            ]
        };
        Skills["monstrous_wave"] = new() {
            Tag = SkillTag.Dash,
            Scales = [
                new(154, 58.3, Element.Physical)
            ]
        };
        Skills["cold_snap"] = new() {
            Tag = SkillTag.Dash,
            Scales = [
                new(292, 118.4)
            ]
        };
        Skills["reef_rock"] = new() {
            Tag = SkillTag.Counter,
            Scales = [
                new(305.5, 341.8)
            ]
        };

        Skills["shark_sentinel"] = new() {
            Tag = SkillTag.QuickAssist,
            Scales = [
                new(243.2, 182.7)
            ]
        };
        Skills["wavefront_impact"] = new() {
            Tag = SkillTag.DefensiveAssist,
            Scales = [
                new (0, 407.7),
                new (0, 530),
                new (0, 250.4),
            ]
        };
        Skills["shark_cruiser"] = new() {
            Tag = SkillTag.FollowUpAssist,
            Scales = [
                new(876.8, 577.3)
            ]
        };

        Skills["drift"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new(101.1, 75.8)
            ]
        };
        Skills["tail_swipe"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(754.5, 608.6)
            ]
        };
        Skills["sharknami"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(1106.6, 557.6)
            ]
        };

        Skills["avalanche"] = new() {
            Tag = SkillTag.Chain,
            Scales = [
                new(1589.9, 533.9)
            ]
        };
        Skills["endless_winter"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [
                new(3781.7, 278.7)
            ]
        };
    }

    public override Stat? ApplyAbilityPassive(string ability) {
        switch (ability) {
            case "arctic_ambush" or "flash_freeze_trimming":
                return new(1, Affix.CritDamage);
            default:
                return null;
        }
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(0.3, Affix.IceDmgBonus)];
        }
        
        return [];
    }
}