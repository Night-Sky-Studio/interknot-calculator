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
        
        var key = ModifierKey.Weapon(Id) + ModifierKey.Passive();
        
        if (!ctx.TryActivateGlobal(key)) return;
        
        foreach (var agent in ctx.Team.Values) {
            agent.DmgBonus.Add(new(key, 0.125 * 2));
            agent.Atk.Add(new(key, 0.1, ModifierType.CombatRatio));
        }
    }
}