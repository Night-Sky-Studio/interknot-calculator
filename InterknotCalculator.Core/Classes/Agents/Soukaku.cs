using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Soukaku : SupportAgent, IAgentReference<Soukaku> {
    public static Soukaku Reference(uint weaponId, uint setId) {
        var soukaku = new Soukaku();
        
        soukaku.InitializeStats(new () {
            [Affix.Atk] = 2500
        });

        soukaku.SetWeaponPassive(weaponId);
        soukaku.SetDriveDiscsPassive(setId);
        
        return soukaku;
    }
    
    public Soukaku() : base(AgentId.Soukaku) {
        Speciality = Speciality.Support;
        Element = Element.Ice;
        Rarity = Rarity.A;
        Faction = Faction.HollowSpecialOperationsSection6;
        
        InitializeStats(new () {
            [Affix.Hp] = 8026,
            [Affix.Def] = 597,
            [Affix.Atk] = 590 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 86,
            [Affix.AnomalyMastery] = 96,
            [Affix.AnomalyProficiency] = 93,
            [Affix.EnergyRegen] = 1.56
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            foreach (var agent in c.Team.Values) {
                agent.Atk.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                    Math.Min(Atk.InitialValue * 2 * 0.2, 1000), ModifierType.CombatFlat));
            }
            
            ElementalDmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.2));
        });
    }
}