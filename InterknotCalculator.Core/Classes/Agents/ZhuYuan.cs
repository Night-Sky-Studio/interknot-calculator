using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class ZhuYuan : Agent {
    private bool IsEnemyStunned { get; set; }
    
    public ZhuYuan() : base(1241) {
        Speciality = Speciality.Attack;
        Element = Element.Ether;
        Rarity = Rarity.S;
        Faction = Faction.CriminalInvestigationSpecialResponseTeam;

        InitializeStats(new () {
            [Affix.Hp] = 7482,
            [Affix.Def] = 600,
            [Affix.Atk] = 919,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.788,
            [Affix.Impact] = 90,
            [Affix.AnomalyMastery] = 93,
            [Affix.AnomalyProficiency] = 92,
            [Affix.EnergyRegen] = 1.2
        });

        Skills["dont_move"] = new(SkillTag.BasicAtk, [
            new(87.1, 32.6, element: Element.Physical),
            new(252.9, 146.8, 104.77),
            new(274.8, 143.1, element: Element.Physical),
            new(302.8, 187, 59.12),
            new(325, 209.6, 60.05)
        ]);
        Skills["please_do_not_resist_phys"] = new(SkillTag.BasicAtk, [
            new(107.6, 90.4, element: Element.Physical),
            new(107.6, 90.4, element: Element.Physical),
            new(322.6, 268.9, element: Element.Physical)
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
            new(107.6, 90.4, element: Element.Physical),
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

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            if (c.Team.Values.Any(a => a.Speciality is Speciality.Support || a.Faction == Faction)) {
                CritRate.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.3));
            }
        });
    }

    public override Stat? ApplyAbilityPassive(Ability ability) {
        if (ability.Name is "please_do_not_resist" or "overwhelming_firepower") {
            return new(Affix.DmgBonus, IsEnemyStunned ? 0.8 : 0.4);
        }
        return null;
    }

    public override IEnumerable<AgentAction> GetActionDamage(Context ctx, Ability ability) {
        IsEnemyStunned = ctx.Enemy.StunMultiplier > 1;

        return base.GetActionDamage(ctx, ability);  
    } 
}