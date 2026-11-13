using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Miyabi : Agent, ICustomAnomaly {
    public Element AnomalyElement { get; set; }
    
    public Miyabi() : base(AgentId.Miyabi) {
        Speciality = Speciality.Anomaly;
        Element = Element.Ice;
        AnomalyElement = Element.Frost;
        Rarity = Rarity.S;
        Faction = Faction.HollowSpecialOperationsSection6;

        InitializeStats(new () {
            [Affix.Hp] = 7673,
            [Affix.Def] = 606,
            [Affix.Atk] = 880,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 86,
            [Affix.AnomalyMastery] = 116,
            [Affix.AnomalyProficiency] = 238,
            [Affix.EnergyRegen] = 1.2
        });

        Anomalies[Element.Frost] = new(1500, Element.Frost);

        Skills["kazahana"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new (54.4, 21.2, element: Element.Physical),
                new (59.3, 41.2, element: Element.Physical),
                new (126.6, 70.6, 62.9),
                new (193.3, 123.9, 91.21),
                new (258.8, 197.0, 129.27)
            ]
        };
        Skills["shimotsuki"] = new() {
            Tag = SkillTag.BasicAtk,
            Scales = [
                new(910.1, 66.0, 39.98),
                new(1717.2, 97.2, 56.68),
                new(4282.8, 567.0, 343.36),
            ]
        };
        
        Skills["fuyubachi"] = new() {
            Tag = SkillTag.Dash,
            Scales = [new(52.2, 19.5, element: Element.Physical)]
        };
        Skills["kan_suzume"] = new() {
            Tag = SkillTag.Counter,
            Scales = [new(492.3, 322.3, 94.99)]
        };
        
        Skills["dancing_petals"] = new() {
            Tag = SkillTag.QuickAssist,
            Scales = [new(209.0, 157.3, 94.99)]
        };
        Skills["drifting_petals"] = new() {
            Tag = SkillTag.DefensiveAssist,
            Scales = [
                new (0, 407.7),
                new (0, 514.4),
                new (0, 193.2)
            ]
        };
        Skills["falling_petals"] = new() {
            Tag = SkillTag.FollowUpAssist,
            Scales = [
                new (676.6, 438.2, 286.17)
            ]
        };
        
        Skills["miyuki"] = new() {
            Tag = SkillTag.Special,
            Scales = [
                new (72.1, 54.5, 32.51)
            ]
        };
        Skills["hisetsu"] = new() {
            Tag = SkillTag.ExSpecial,
            Scales = [
                new(788.3, 483.4, 157.75 + 250.52),
                new(967.2, 608.5, 189.25 + 297.77)
            ]
        };
        
        Skills["springs_call"] = new() {
            Tag = SkillTag.Chain,
            Scales = [new(1258.3, 284.7, 111.36 * 2 + 148.48)]
        };
        Skills["lingering_snow"] = new() {
            Tag = SkillTag.Ultimate,
            Scales = [new(4776.1, 556.3, 1137.16)]
        };
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        var corePassiveKey = ModifierKey.Agent(Id) + ModifierKey.CorePassive();
        
        ctx.Events.OnCalculationStarted += c => {
            ElementalDmgBonus.Add(corePassiveKey, 0.3, ModifierType.CombatFlat);

            if (c.HasTeammates(a => a.Faction == Faction || a.Speciality is Speciality.Support or Speciality.Anomaly, Id)) {
                DmgBonus.Add(corePassiveKey, 0.6, ModifierType.CombatFlat,
                    filter: AbilityFilter.Of("shimotsuki"));
                ElementalResPen.Add(corePassiveKey, 0.3, ModifierType.CombatFlat,
                    filter: AbilityFilter.Of("shimotsuki"));
            }

            // While the enemy is under Frostburn, all squad members' Anomaly Buildup Rate increases by 20%
            foreach (var agent in c.Team.Values) {
                agent.AnomalyBuildupBonus.Add(corePassiveKey + ModifierKey.AbilityPassive(),
                    0.2, ModifierType.CombatFlat,
                    when: () => c.Enemy.AfflictedAnomaly?.Element is Element.Frost);
            }
        };
    }
}

public class MiyabiM1 : Miyabi {
    private List<Modifier> M1Buffs { get; set; } = [];
    
    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted += c => {
            // While in Shimotsuki Stance, every 1 point of Fallen Frost consumed will allow
            // Basic Attack: Shimotsuki to ignore 6% of DEF, stacking up to 6 times
            // 
            // Remarks: Fallen Frost stacks aren't tracked, this bonus is always active at max 6 stacks
            DefPen.Add(ModifierKey.Agent(Id) + ModifierKey.Mindscape(1) + ModifierKey.AbilityPassive(), 
                0.06 * 6, ModifierType.CombatFlat,
                filter: AbilityFilter.Of("shimotsuki"));
            
            M1Buffs = c.Team.Values
                .Select(agent => agent.AnomalyBuildupBonus.Add(
                    ModifierKey.Agent(Id) + ModifierKey.Mindscape(1) + ModifierKey.CorePassive(), 
                    0.2, ModifierType.CombatFlat, active: false))
                .ToList();
        };
        
        ctx.Events.OnActionExecuting += (c, e) => {
            // When the slash for charge level three of Shimotsuki hits an enemy under Frostburn,
            // it will immediately remove the Frostburn state and increase all squad members'
            // Anomaly Buildup Rate by 20%.
            // Removing Frostburn also ends the base Frostburn buff, which is gated on it.
            if (e.Ability is not { Name: "shimotsuki", Scale: 2 } || 
                c.Enemy.AfflictedAnomaly?.Element is not Element.Frost) return;
            c.Enemy.AfflictedAnomaly = null;
            
            foreach (var buff in M1Buffs) {
                buff.Active = true;
            }
        };
    }
}

public class MiyabiM2 : MiyabiM1 {
    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted += _ => {
            // Basic Attack: Kazahana and Dodge Counter DMG increases by 30%.
            DmgBonus.Add(ModifierKey.Agent(Id) + ModifierKey.Mindscape(2) + ModifierKey.AbilityPassive(), 
                0.3, ModifierType.CombatFlat,
                filter: AbilityFilter.Of("kazahana"));
            DmgBonus.Add(ModifierKey.Agent(Id) + ModifierKey.Mindscape(2) + ModifierKey.AbilityPassive(), 
                0.6, ModifierType.CombatFlat,
                filter: AbilityFilter.Of("kan_suzume"));
            
            // Upon entering the battlefield, Hoshimi Miyabi immediately obtains 6 points
            // of Fallen Frost and her CRIT Rate increases by 15%.
            CritRate.Add(ModifierKey.Agent(Id) + ModifierKey.Mindscape(2) + ModifierKey.CorePassive(), 
                0.15, ModifierType.CombatFlat);
        };
    }
}