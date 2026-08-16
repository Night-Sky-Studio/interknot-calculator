using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class YeShunguang : Agent, IEtherVeilAgent<Verdict> {
    protected double VeilVulnerabilityCap { get; }
    protected int MaxQingmingSwordForce { get; }
    protected int MaxBearer { get; }
    
    public YeShunguang() : base(AgentId.YeShunguang) {
        Speciality = Speciality.Attack;
        Element = Element.HonedEdge;
        Rarity = Rarity.S;
        Faction = Faction.YunkuiSummit;
        
        Stats[Affix.Hp] = 7673;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 938;
        Stats[Affix.CritRate] = 0.194;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 83;
        Stats[Affix.AnomalyMastery] = 94;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;

        VeilVulnerabilityCap = 1.1;
        MaxQingmingSwordForce = 6;
        MaxBearer = 3;
        
        Skills["swiftedge"] = new(SkillTag.BasicAtk, [
            new(159.7, 40.8, element: Element.Physical, energy: 1.734),
            new(488.3, 117.1, element: Element.Physical, energy: 5.07),
            new(250, 54.1, element: Element.Physical, energy: 2.315),
            new(736.5, 161.2, element: Element.Physical, energy: 7.019),
        ]);
        Skills["cloudstream_sword_will"] = new(SkillTag.BasicAtk, [new(234.4, 55.4, element: Element.Physical)]);
        Skills["enlightened_mind_splitting_currents"] = new(SkillTag.BasicAtk, [
            new(225.9, 54.5, 65),
            new(312.1, 74.8, 90),
            new(433.3, 104, 125),
        ]);
        Skills["enlightened_mind_skyward_ascent"] = new(SkillTag.BasicAtk, [new(182.3, 54.5, 32.5)]);
        Skills["enlightened_mind_sunderlight_maximum"] = new(SkillTag.BasicAtk, [new(1209.8, 115.5, 140)]);
        Skills["enlightened_mind_sunderlight"] = new(SkillTag.BasicAtk, [
            new(241.7, 51.6, 61.66),
            new(313.8, 66, 80),
        ]);
        Skills["enlightened_mind_sunderlight_annihilation"] = new(SkillTag.BasicAtk, [
            new(320.5, 68.1, 81.69),
            new(1783.3, 87.5, 105.03),
        ]);
        Skills["phantasm_dash"] = new(SkillTag.Dash, [new(210.6, 31.7, element: Element.Physical, energy: 1.35)]);
        Skills["swallow_strike"] = new(SkillTag.Counter, [new(692.8, 176.8, element: Element.Physical, energy: 4.08)]);
        Skills["illuminating_darkness"] = new(SkillTag.Entry, [new(800.8, 181.5, 220)]);
        Skills["support_guard"] = new(SkillTag.QuickAssist, [new(161.9, 48.2, element: Element.Physical, energy: 2.07)]);
        Skills["enlightened_mind_tactical_support"] = new(SkillTag.QuickAssist, [new(135.5, 50.7, 60)]);
        Skills["when_i_return"] = new(SkillTag.DefensiveAssist, [
            new(0, 293.2),
            new(0, 311.7),
            new(0, 179.7),
        ]);
        Skills["enlightened_mind_unification_illuminating_darkness"] = new(SkillTag.FollowUpAssist, [new(800.8, 181.5, 220)]);
        Skills["cease_hostility"] = new(SkillTag.FollowUpAssist, [new(828.4, 210.4, element: Element.Physical)]);
        Skills["enlightened_mind_unification"] = new(SkillTag.FollowUpAssist, [new(860.1, 219.7, 286.22)]);
        Skills["guiding_tides"] = new(SkillTag.Special, [
            new(197.9, 37.5, element: Element.Physical),
            new(632.7, 199.2, element: Element.Physical),
        ]);
        Skills["enlightened_mind_clean_exit"] = new(SkillTag.Special, [new(142.5, 26.3, 31.66)]);
        Skills["gale_suppression"] = new(SkillTag.ExSpecial, [new(2357.6, 510.6, element: Element.Physical, energy: -60)]);
        Skills["enlightened_mind_soaring_light"] = new(SkillTag.ExSpecial, [
            new(352.7, 27.2, 32.78),
            new(2116.2, 163.2, 196.66),
        ]);
        Skills["enlightened_mind_return_to_dust"] = new(SkillTag.ExSpecial, [new(2322.7, 209.8, 253.33)]);
        Skills["smite_the_wicked"] = new(SkillTag.Chain, [new(1743.9, 272.8, element: Element.Physical)]);
        Skills["enlightened_mind_lure_thunder"] = new(SkillTag.Chain, [new(1817.2, 206.8, 449.5)]);
        Skills["chasing_storms"] = new(SkillTag.Ultimate, [new(3850, 82.5, 100)]);
        Skills["cleaving_heavens"] = new(SkillTag.Ultimate, [new(6168.7, 227.2, 275)]);

        Macros["#enlightened_mind_combo"] = [
            "enlightened_mind_sunderlight 1",
            "enlightened_mind_sunderlight 2",
            "enlightened_mind_sunderlight_annihilation 2",
            "enlightened_mind_sunderlight_maximum",
            "enlightened_mind_soaring_light",
            "enlightened_mind_skyward_ascent"
        ];
    }

    private bool IsTeamPassiveActive { get; set; } = false;

    private bool IsEnlightenedMind { get; set; } = false;
    
    public int QingmingSwordForce {
        get;
        set => field = Math.Clamp(value, 0, MaxQingmingSwordForce);
    }
    private int Bearer {
        get;
        set => field = Math.Clamp(value, 0, MaxBearer);
    }
    
    private void GainSwordForce(int amount) {
        if (IsEnlightenedMind) {
            // While in Enlightened Mind, gained Force converts to Bearer stacks
            Bearer += amount;
            return;
        }

        var overflow = QingmingSwordForce + amount - MaxQingmingSwordForce;
        QingmingSwordForce += amount;
        if (overflow > 0) Bearer += overflow;
    }
    
    private void EnterEnlightenedMind(Context ctx) {
        if (QingmingSwordForce != 6)
            throw new InvalidOperationException("Cannot enter Enlightened Mind with less than 6 Qingming Sword Force");
        
        if (!IsEnlightenedMind) IsEnlightenedMind = true;
        if (ctx.GetEtherVeil<Verdict>() is { } existing) {
            ctx.DeactivateEtherVeil(this, existing);
        }
        ctx.ActivateEtherVeil(this, EtherVeil);
    }

    private void ExitEnlightenedMind(Context ctx) {
        if (!IsEnlightenedMind) 
            throw new InvalidOperationException("Cannot exit Enlightened Mind without entering it first");
        
        if (ctx.GetEtherVeil<Verdict>() is { } verdict) {
            ctx.DeactivateEtherVeil(this, verdict);
        }
        if (IsEnlightenedMind) IsEnlightenedMind = false;

        // Bearer stacks are converted back into Qingming Sword Force
        QingmingSwordForce += Bearer;
        Bearer = 0;
    }
    
    public override void ApplyPassive() {
        // Core Passive: Burning Clarity
        BonusStats[Affix.CritRate] += 0.3;
        BonusStats[Affix.DmgBonus] += 0.25;
    }
    
    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnActionExecuted.Add((c, e) => {
            if (e.Agent != this) return;

            switch (e.Ability.Name) {
                // Ultimate: Chasing Storms grants 6 Qingming Sword Force,
                // then enters Enlightened Mind and activates "Ether Veil: Verdict"
                case "chasing_storms":
                    GainSwordForce(6);
                    EnterEnlightenedMind(c);
                    break;

                // Entry Skill: Illuminating Darkness requires full Qingming Sword Force
                case "illuminating_darkness":
                    EnterEnlightenedMind(c);
                    break;

                // Qingming Sword Force consumption
                case "enlightened_mind_soaring_light" or "enlightened_mind_sunderlight_maximum":
                    QingmingSwordForce -= 1;
                    break;
                case "enlightened_mind_sunderlight_annihilation" when e.Ability.Scale == 1:
                    QingmingSwordForce -= 2;
                    break;
            }
        });

        // Additional Ability: Shadowtrace Flight 
        // when a squadmate activates any Ether Veil, gain 3 Qingming Sword Force
        // (converted to Bearer stacks if already in Enlightened Mind)
        ctx.Events.OnEtherVeilActivated.Add((_, e) => {
            if (!IsTeamPassiveActive || e.Agent == this) return;
            GainSwordForce(3);
        });
    }

    protected double OriginalEnemyStunMultiplier { get; set; } = 1;
    
    public override IEnumerable<AgentAction> GetActionDamage(Context ctx, Ability ability) {
        OriginalEnemyStunMultiplier = ctx.Enemy.StunMultiplier; // 1

        // Core Passive: Burning Clarity
        // Veil Vulnerability: while "Ether Veil: Verdict" is active, the enemy's Stun DMG multiplier
        // is ignored and replaced with the Veil Vulnerability bonus, capped at 110%.
        if (IsEnlightenedMind && ctx.GetEtherVeil<Verdict>() is not null) {
            ctx.Enemy.StunMultiplier = Math.Min(1 + VeilVulnerabilityCap, OriginalEnemyStunMultiplier + 0.5);
        }

        try {
            return base.GetActionDamage(ctx, ability).ToList();
        } finally {
            ctx.Enemy.StunMultiplier = OriginalEnemyStunMultiplier;

            // Exiting Enlightened Mind removes "Ether Veil: Verdict"
            // after the finisher's damage has been resolved
            if (ability.Name is "cleaving_heavens" or "enlightened_mind_return_to_dust") {
                ExitEnlightenedMind(ctx);
            }
        }
    }
    
    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        // Additional Ability: Shadowtrace Flight 
        // requires a Support or Defense character in the squad
        if (team.Any(a => a.Speciality is Speciality.Support or Speciality.Defense)) {
            IsTeamPassiveActive = true;
        }

        return [];
    }
    
    public Verdict EtherVeil { get; } = new();
}