using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Nicole : SupportAgent, IAgentReference<Nicole> {
    public static Nicole Reference(uint weaponId, uint setId) {
        var nicole = new Nicole();
        
        nicole.InitializeStats(new () {
            [Affix.Hp] = 10000,
            [Affix.Atk] = 2400,
            [Affix.Def] = 800,
            [Affix.AnomalyProficiency] = 320
        });
        
        nicole.SetWeaponPassive(weaponId);
        nicole.SetDriveDiscsPassive(setId);
        
        return nicole;
    }
    
    public Nicole() : base(AgentId.Nicole) {
        Speciality = Speciality.Support;
        Element = Element.Ether;
        Rarity = Rarity.A;
        Faction = Faction.CunningHares;
        
        InitializeStats(new () {
            [Affix.Hp] = 8145,
            [Affix.Def] = 622,
            [Affix.Atk] = 574 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 88,
            [Affix.AnomalyMastery] = 93,
            [Affix.AnomalyProficiency] = 90,
            [Affix.EnergyRegen] = 1.56,
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);

        ctx.Events.OnCalculationStarted.Add(c => {
            foreach (var agent in c.Team.Values) {
                agent.ResPen.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.4));
                
                // M6
                agent.CritRate.Add(new(ModifierKey.Agent(Id) + ModifierKey.Mindscape(6) 
                                                             + ModifierKey.CorePassive(), 0.15));
            }

            if (c.Team.Values.Any(a => a.Element.Matches(Element) || a.Faction == Faction)) {
                ElementalDmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.25));
            }
        });
    }
}