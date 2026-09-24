using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class WeepingCradle : Weapon {
    public WeepingCradle() : base(WeaponId.WeepingCradle) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 684);
        SecondaryStat = new(Affix.PenRatio, 0.24);
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var key = ModifierKey.Weapon(Id) + ModifierKey.CorePassive();
        
        if (!ctx.TryActivateGlobal(key)) return;
        
        foreach (var agent in ctx.Team.Values) {
            agent.DmgBonus.Add(new(key, 0.202));
        }
    }
}
