using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class TheVault : Weapon {
    public TheVault() : base(WeaponId.TheVault) {
        Speciality = Speciality.Support;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.5);
        Passive = [new(Affix.EnergyRegen, 0.8)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var key = ModifierKey.Weapon(Id) + ModifierKey.CorePassive();

        if (!ctx.TryActivateGlobal(key)) return;

        foreach (var agent in ctx.Team.Values) {
            agent.DmgBonus.Add(new(key, 0.24));
        }
    }
}
