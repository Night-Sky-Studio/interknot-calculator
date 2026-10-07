using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class JuFufu : SupportAgent, IAgentReference<JuFufu> {
    public static JuFufu Reference(uint weaponId, uint setId) {
        var fufu = new JuFufu();
        
        fufu.InitializeStats(new () {
            [Affix.Atk] = 3400,
            [Affix.CritRate] = 0.5
        });

        fufu.SetWeaponPassive(weaponId);
        fufu.SetDriveDiscsPassive(setId);
        
        return fufu;
    }
    
    public JuFufu() : base(AgentId.JuFufu) {
        Speciality = Speciality.Stun;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.YunkuiSummit;
        
        InitializeStats(new () {
            [Affix.Hp] = 8250,
            [Affix.Def] = 765,
            [Affix.Atk] = 597,
            [Affix.CritRate] = 0.194,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 118,
            [Affix.AnomalyMastery] = 93,
            [Affix.AnomalyProficiency] = 96,
            [Affix.EnergyRegen] = 1.2
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            // every 100 atk over 2800, crit dmg + 0.05
            const double atkThreshold = 2800;
            const double atkStep = 100;
            const double critDmgPerStep = 0.05;
            const double bonusCap = 0.3;
            
            foreach (var agent in c.Team.Values) {
                var atkOverThreshold = Math.Max(Atk.InitialValue - atkThreshold, 0);
                var steps = atkOverThreshold / atkStep;
                var critDmgBonus = 0.2 + Math.Min(steps * critDmgPerStep, bonusCap);
                
                agent.CritDamage.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                    critDmgBonus, ModifierType.CombatFlat));
                agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                    0.2, ModifierType.CombatFlat, SkillTag.Chain));
                agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                    0.4, ModifierType.CombatFlat, SkillTag.Ultimate));
            }
            
            Impact.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 50, ModifierType.CombatFlat));
        });
    }
}