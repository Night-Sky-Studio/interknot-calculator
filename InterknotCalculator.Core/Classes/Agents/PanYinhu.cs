using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class PanYinhu : SupportAgent, IAgentReference<PanYinhu> {
    public static PanYinhu Reference(uint weaponId, uint setId) {
        var panYinhu = new PanYinhu();
        
        panYinhu.InitializeStats(new () {
            [Affix.Atk] = 3000
        });

        panYinhu.SetWeaponPassive(weaponId);
        panYinhu.SetDriveDiscsPassive(setId);
        
        return panYinhu;
    }

    public PanYinhu() : base(AgentId.PanYinhu) {
        Speciality = Speciality.Support;
        Element = Element.Physical;
        Rarity = Rarity.A;
        Faction = Faction.YunkuiSummit;
        
        InitializeStats(new () {
            [Affix.Hp] = 8453,
            [Affix.Atk] = 661,
            [Affix.Def] = 712,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 94,
            [Affix.AnomalyMastery] = 91,
            [Affix.AnomalyProficiency] = 90,
            [Affix.EnergyRegen] = 1.56
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            foreach (var agent in c.Team.Values) {
                if (agent is RuptureAgent a) {
                    // M6
                    a.SheerForce.Add(new(ModifierKey.Agent(Id) + ModifierKey.Mindscape(6) + ModifierKey.CorePassive(),
                        Math.Min((0.18 + 0.06) * Atk.InitialValue, 540 + 180)));
                }
            }

            if (c.Team.Values.Any(a => a.Speciality is Speciality.Rupture || a.Faction == Faction)) {
                foreach (var agent in c.Team.Values) {
                    // M1
                    agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.Mindscape(1) + ModifierKey.TeamPassive(), 0.2 + 0.1));
                }   
            }
        });
    }
}