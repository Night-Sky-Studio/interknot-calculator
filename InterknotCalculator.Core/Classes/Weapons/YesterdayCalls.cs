using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class YesterdayCalls : Weapon {
    public YesterdayCalls() : base(WeaponId.YesterdayCalls) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [new(Affix.DazeBonus, 0.27)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var key = ModifierKey.Weapon(Id) + ModifierKey.CorePassive();

        if (!ctx.TryActivateGlobal(key)) return;

        foreach (var agent in ctx.Team.Values) {
            agent.CritDamage.Add(new(key, 0.3));
        }
    }
}