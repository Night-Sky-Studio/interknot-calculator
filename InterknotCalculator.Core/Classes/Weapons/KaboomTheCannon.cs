using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class KaboomTheCannon : Weapon {
    public KaboomTheCannon() : base(WeaponId.KaboomTheCannon) {
        Speciality = Speciality.Support;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.5);
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        var key = ModifierKey.Weapon(Id) + ModifierKey.CorePassive();

        if (!ctx.TryActivateGlobal(key)) return;
        
        foreach (var agent in ctx.Team.Values) {
            agent.Atk.Add(new(key, 0.16, ModifierType.CombatRatio));
        }
    }
}
