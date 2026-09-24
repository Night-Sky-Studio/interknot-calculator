using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Rina : SupportAgent, IAgentReference<Rina> {
    public static Rina Reference(uint weaponId, uint setId) {
        var rina = new Rina();
        
        rina.InitializeStats(new () {
            [Affix.Atk] = 2600
        });
        rina.PenRatio.Add(new(ModifierKey.Agent(AgentId.Rina) + ModifierKey.CorePassive(), 0.3));

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
        
        ctx.Events.OnCalculationStarted.Add(c => {
            foreach (var agent in c.Team.Values) {
                agent.PenRatio.Add(new(ModifierKey.Agent(AgentId.Rina) + ModifierKey.CorePassive(), 
                    Math.Min(PenRatio * 0.25 + 0.12, 0.3)));
            }

            if (c.Team.Values.Any(a => a.Element.Matches(Element) || a.Faction == Faction)) {
                foreach (var agent in c.Team.Values) {
                    if (agent.Element.Matches(Element.Electric)) {
                        agent.ElementalDmgBonus.Add(new(ModifierKey.Agent(AgentId.Rina) + ModifierKey.TeamPassive(), 0.1));
                    }
                }
            }
        });
    }
}