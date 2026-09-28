using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class HalfSugarBunny : Weapon {
    public HalfSugarBunny() : base(WeaponId.HalfSugarBunny) {
        Speciality = Speciality.Defense;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.HpRatio, 0.3);
    }

    private bool EtherVeilBonusActive { get; set; }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        var key = ModifierKey.Weapon(Id) + ModifierKey.Passive();
        
        ctx.Events.OnCalculationStarted.Add(c => {
            if (!c.TryActivateGlobal(key)) return;
            
            foreach (var agent in c.Team.Values) {
                agent.Atk.Add(new(key, 0.1, ModifierType.CombatRatio));
                agent.MaxHp.Add(new(key, 0.1, ModifierType.CombatRatio));
            }
        });
        
        ctx.Events.OnEtherVeilActivated.Add((c, _) => {
            if (EtherVeilBonusActive) return;

            foreach (var (_, agent) in c.Team) {
                agent.CritDamage.Add(new(key, 0.3, ModifierType.CombatFlat));
            }
            
            EtherVeilBonusActive = true;
        });
    }
}