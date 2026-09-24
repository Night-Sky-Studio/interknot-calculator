using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class BashfulDemon : Weapon {
    public BashfulDemon() : base(WeaponId.BashfulDemon) {
        Speciality = Speciality.Support;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.AtkRatio, 0.25);
        Passive = [new(Affix.IceDmgBonus, 0.24)];
    }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        var key = ModifierKey.Weapon(Id) + ModifierKey.CorePassive();
        if (!ctx.TryActivateGlobal(key)) return;

        foreach (var agent in ctx.Team.Values) {
            agent.Atk.Add(new(key, 0.128, ModifierType.CombatRatio));
        }
    }
}
