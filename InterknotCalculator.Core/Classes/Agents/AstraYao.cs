using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class AstraYao : SupportAgent, IAgentReference<AstraYao> {
    public static AstraYao Reference(uint weaponId, uint setId) {
        var astraYao = new AstraYao();
        
        astraYao.InitializeStats(new () {
            [Affix.Atk] = 3430
        });

        astraYao.SetWeaponPassive(weaponId);
        astraYao.SetDriveDiscsPassive(setId);
        
        return astraYao;
    }
    public AstraYao() : base(AgentId.AstraYao) {
        Speciality = Speciality.Support;
        Element = Element.Ether;
        Rarity = Rarity.S;
        Faction = Faction.StarsOfLyra;

        InitializeStats(new() {
            [Affix.Hp] = 7788,
            [Affix.Def] = 600,
            [Affix.Atk] = 640 + 75,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 83,
            [Affix.AnomalyMastery] = 93,
            [Affix.AnomalyProficiency] = 92,
            [Affix.EnergyRegen] = 1.56    
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);

        ctx.Events.OnCalculationStarted.Add(c => {
            var key = ModifierKey.Agent(Id) + ModifierKey.CorePassive();

            if (!c.TryActivateGlobal(key)) return;

            foreach (var agent in c.Team.Values) {
                agent.Atk.Add(new(key, Math.Min(Atk * 0.35, 1200), ModifierType.CombatFlat));
                agent.DmgBonus.Add(new(key, 0.2));
                agent.CritDamage.Add(new(key, 0.25));
            }
        });
    }
}