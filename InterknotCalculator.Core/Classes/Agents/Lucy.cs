using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Lucy : SupportAgent, IAgentReference<Lucy> {
    public static Lucy Reference(uint weaponId, uint setId) {
        var lucy = new Lucy();
        
        lucy.InitializeStats(new () {
            [Affix.Atk] = 1932
        });
        
        lucy.SetWeaponPassive(weaponId);
        lucy.SetDriveDiscsPassive(setId);

        return lucy;
    }
    
    public Lucy() : base(AgentId.Lucy) {
        Speciality = Speciality.Support;
        Element = Element.Fire;
        Rarity = Rarity.A;
        Faction = Faction.SonsOfCalydon;

        InitializeStats(new () {
            [Affix.Hp] = 8025,
            [Affix.Def] = 612,
            [Affix.Atk] = 583 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 86,
            [Affix.AnomalyMastery] = 93,
            [Affix.AnomalyProficiency] = 94,
            [Affix.EnergyRegen] = 1.56
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            foreach (var agent in c.Team.Values) {
                // M6
                agent.Atk.Add(new(ModifierKey.Agent(Id) + ModifierKey.Mindscape(6) + ModifierKey.CorePassive(), 
                    Math.Min(Atk.InitialValue * 0.258 + 104, 600)));
                
                // M4
                agent.CritDamage.Add(new(ModifierKey.Agent(Id) + ModifierKey.Mindscape(4) 
                                                               + ModifierKey.CorePassive(), 0.1));
            }
        });
    }
}