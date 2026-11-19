using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Rina : SupportAgent, IAgentReference<Rina> {
    public static Rina Reference(uint weaponId, uint setId) {
        var rina = new Rina();
        
        rina.InitializeStats(new () {
            [Affix.PenRatio] = 0.704 // with STR6 should be 72%
        });

        rina.SetWeaponPassive(weaponId);
        rina.SetDriveDiscsPassive(setId);
        
        return rina;
    }
    public Rina() : base(AgentId.Rina) {
        Speciality = Speciality.Support;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.VictoriaHousekeeping;
        
        InitializeStats(new () {
            [Affix.Hp] = 8609,
            [Affix.Def] = 600,
            [Affix.Atk] = 642 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 83,
            [Affix.AnomalyMastery] = 92,
            [Affix.AnomalyProficiency] = 93,
            [Affix.EnergyRegen] = 1.2,
            [Affix.PenRatio] = 0.144
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted += c => {
            // STR 6:
            // PEN Ratio increases by 1.6%.
            PenRatio.Add(ModifierKey.Agent(AgentId.Rina) + ModifierKey.Potential(6), 0.016, ModifierType.CombatFlat);
            
            // Core Passive: Mini Destruction Partner
            // When ordering Drusilla or Anastella to attack, other squad members' PEN Ratio
            // increases together with Rina's, by 25% of Rina's PEN Ratio + 12%, up to 30%.

            // STR 6:
            // While the buff gained from Core Passive: Mini Destruction Partner is active,
            // for every 1% of Rina's PEN Ratio, all squad members gain 8 ATK and 6.5 DEF,
            // up to a maximum of 576 ATK and 468 DEF.
            foreach (var agent in c.Team.Values) {
                if (agent.Id != Id) {
                    agent.PenRatio.Add(ModifierKey.Agent(AgentId.Rina) + ModifierKey.CorePassive(),
                        Math.Min(PenRatio * 0.25 + 0.12, 0.3), ModifierType.CombatFlat);
                }
                
                // yes, STR6 buff applies to everyone, including Rina, but Pen% only to team members
                agent.Atk.Add(ModifierKey.Agent(AgentId.Rina) + ModifierKey.Potential(6), 
                    Math.Min(PenRatio * 100 * 8, 576), ModifierType.CombatFlat);
                agent.Def.Add(ModifierKey.Agent(AgentId.Rina) + ModifierKey.Potential(6), 
                    Math.Min(PenRatio * 100 * 6.5, 468), ModifierType.CombatFlat);
            }

            // When another character in your squad shares the same Attribute or Faction
            if (c.HasTeammates(a => a.Element.Matches(Element) || a.Faction == Faction, Id)) {
                // When Shocked enemies are on the field, all squad members' Electric DMG increases by 10%.
                foreach (var agent in c.Team.Values) {
                    agent.GetStat(Affix.ElectricDmgBonus).Add(ModifierKey.Agent(AgentId.Rina) + ModifierKey.TeamPassive(), 
                        0.1, ModifierType.CombatFlat,
                        when: () => c.Enemy.AfflictedAnomaly?.Element.Matches(Element.Electric) ?? false);
                }
            }
        };
    }
}