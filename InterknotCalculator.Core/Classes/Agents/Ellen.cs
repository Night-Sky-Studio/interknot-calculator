using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public class Ellen : Agent {
    public Ellen() : base(1191) {
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

        Skills["saw_teeth_trimming"] = new(SkillTag.BasicAtk, [
            new(98.3, 37.6, element: Element.Physical),
            new(222.2, 130.8, element: Element.Physical),
            new(478.6, 306.8, element: Element.Physical),
        ]);
        Skills["flash_freeze_trimming"] = new(SkillTag.BasicAtk, [
            new(199.7, 74.1, 44.35),
            new(368.8, 135.3, 81.99),
            new(993.4, 368.7, 223.17),
        ]);

        // STR 6
        Skills["glacial_blade_wave"] = new(SkillTag.BasicAtk, [
            new(461.2, 101.1, 121.66),
            new(294.2, 127.1, 49.96)
        ]);
        Skills["icy_blade"] = new(SkillTag.BasicAtk, [
            new(362.7, 117, 23.38 * 3 + 5.01 * 0),
            new(441.3, 143.7, 23.38 * 3 + 5.01 * 1),
            new(519.9, 170.4, 23.38 * 3 + 5.01 * 2)
        ]);

        Skills["arctic_ambush"] = new(SkillTag.Dash, [
            new(125, 94.2, 56.59),
            new(255.2, 147.7, 89.19),
            new(316.6, 183.3, 110.56)
        ]);
        Skills["monstrous_wave"] = new(SkillTag.Dash, [
            new(154, 58.3, element: Element.Physical)
        ]);
        Skills["cold_snap"] = new(SkillTag.Dash, [
            new(292, 118.4, 71.63)
        ]);
        Skills["reef_rock"] = new(SkillTag.Counter, [
            new(305.5, 341.8, 106.7)
        ]);

        Skills["shark_sentinel"] = new(SkillTag.QuickAssist, [
            new(243.2, 182.7, 110.03)
        ]);
        Skills["wavefront_impact"] = new(SkillTag.DefensiveAssist, [
            new(0, 407.7),
            new(0, 530),
            new(0, 250.4),
        ]);
        Skills["shark_cruiser"] = new(SkillTag.FollowUpAssist, [
            new(876.8, 577.3, 373.92)
        ]);

        Skills["drift"] = new(SkillTag.Special, [
            new(101.1, 75.8, 45.84)
        ]);
        Skills["tail_swipe"] = new(SkillTag.ExSpecial, [
            new(754.5, 608.6, 403.73)
        ]);
        Skills["sharknami"] = new(SkillTag.ExSpecial, [
            new(1106.6, 557.6, 372.19)
        ]);

        Skills["avalanche"] = new(SkillTag.Chain, [
            new(1589.9, 533.9, 522.83)
        ]);
        Skills["endless_winter"] = new(SkillTag.Ultimate, [
            new(3781.7, 278.7, 168.33)
        ]);
    }

    public override Stat? ApplyAbilityPassive(Ability ability) {
        switch (ability.Name) {
            case "flash_freeze_trimming":
                return new(Affix.CritDamage, 1);
            default:
                return null;
        }
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [
                new(Affix.IceDmgBonus, 0.3),
                // STR 6
                new(Affix.CritDamage, 0.48),
                new(Affix.IceResPen, 0.1)
            ];
        }

        return [];
    }
}

public class EllenM1 : Ellen {
    public override void ApplyPassive() {
        base.ApplyPassive();
        
        // For each point of Flash Freeze Charge consumed, Ellen’s CRIT Rate
        // is increased by 2% for 15s, stacking up
        BonusStats[Affix.IceDmgBonus] += 0.02 * 6;
    }
}

public class EllenM2 : EllenM1 {
    public override Stat? ApplyAbilityPassive(Ability ability) {
        if (ability.Tag is SkillTag.ExSpecial) {
            return new(Affix.CritDamage, 0.6);
        }

        return base.ApplyAbilityPassive(ability);
    }
}

public class EllenM3: EllenM2 { }
public class EllenM4: EllenM3 { }
public class EllenM5: EllenM4 { }

public class EllenM6 : EllenM5 {
    public EllenM6() : base() {
        Skills["saw_teeth_trimming"] = new(SkillTag.BasicAtk, [
            new(116.3, 42.4, element: Element.Physical, energy: 0.679),
            new(262.6, 146.8, element: Element.Physical, energy: 2.415),
            new(703.8, 405.4, element: Element.Physical, energy: 6.688),
        ]);
        Skills["flash_freeze_trimming"] = new(SkillTag.BasicAtk, [
            new(236.1, 83.3, 44.35, 1.358),
            new(436, 151.7, 81.99, 2.509),
            new(1174.2, 413.5, 223.17, 6.428),
        ]);
        Skills["glacial_blade_wave"] = new(SkillTag.BasicAtk, [
            new(545.2, 113.5, 121.66, 1.862),
            new(347.8, 142.7, 49.96, 2.341),
        ]);
        Skills["icy_blade"] = new(SkillTag.BasicAtk, [
            new(428.7, 131.4, 23.38 * 3, 2.526),
            new(521.7, 161.7, 23.38 * 3 + 5.01 * 3, 3.069),
            new(614.7, 192, 23.38 * 3 + 5.01 * 6, 3.612),
        ]);
        Skills["arctic_ambush"] = new(SkillTag.Dash, [
            new(147.8, 105.8, 56.59, 2.038),
            new(301.6, 165.7, 89.19, 3.211),
            new(374.2, 205.7, 110.56, 3.981),
        ]);
        Skills["monstrous_wave"] = new(SkillTag.Dash, [new(182, 65.5, element: Element.Physical, energy: 1.26)]);
        Skills["cold_snap"] = new(SkillTag.Dash, [new(345.2, 132.8, 71.63, 2.579)]);
        Skills["reef_rock"] = new(SkillTag.Counter, [new(361.1, 383.4, 106.7, 3.842)]);
        Skills["shark_sentinel"] = new(SkillTag.QuickAssist, [new(287.6, 205.1, 110.03, 3.962)]);
        Skills["wavefront_impact"] = new(SkillTag.DefensiveAssist, [
            new(0, 457.3),
            new(0, 576.8),
            new(0, 280.8),
        ]);
        Skills["shark_cruiser"] = new(SkillTag.FollowUpAssist, [new(1036.4, 647.3, 373.92)]);
        Skills["drift"] = new(SkillTag.Special, [new(119.5, 85, 45.84)]);
        Skills["tail_swipe"] = new(SkillTag.ExSpecial, [new(891.7, 682.6, 403.73, -40)]);
        Skills["sharknami"] = new(SkillTag.ExSpecial, [new(1307.8, 625.2, 372.19, -40)]);
        Skills["avalanche"] = new(SkillTag.Chain, [new(1879.1, 598.7, 522.83)]);
        Skills["endless_winter"] = new(SkillTag.Ultimate, [new(4469.3, 312.7, 168.33)]);
    }
    
    public override void ApplyPassive() {
        base.ApplyPassive();

        BonusStats[Affix.PenRatio] += 0.2;
    }

    private int FeastStacks { get; set => field = Math.Min(value, 3); } = 0;

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnActionExecuted.Add((c, e) => {
            // When Ellen uses an EX Special Attack, Chain Attack, or gains Quick Charge [...]
            // She gains 1 stack of The Feast Begins, up to a maximum of 3 stacks.

            if (e.Ability.Name is "icy_blade" or "vortex" or "shark_sentinel") {
                FeastStacks++;
            }
        });
    }

    public override Stat? ApplyAbilityPassive(Ability ability) {
        if (ability.Name is "arctic_ambush" && FeastStacks == 3) {
            return new(Affix.DmgBonus, 2.5);
        }
        
        return base.ApplyAbilityPassive(ability);
    }
}
