using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class Thoughtbop : Weapon {
    public Thoughtbop() : base(WeaponId.Thoughtbop) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.6);
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            var key = ModifierKey.Weapon(Id) + ModifierKey.Passive();
        
            if (!c.TryActivateGlobal(key)) return;
        
            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.Add(new(key, 0.125 * 2, ModifierType.CombatFlat));
                agent.Atk.Add(new(key, 0.1, ModifierType.CombatRatio));
            }
        });
    }
}