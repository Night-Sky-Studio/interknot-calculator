using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class ElegantVanity : Weapon {
    public ElegantVanity() : base(WeaponId.ElegantVanity) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        foreach (var agent in ctx.Team.Values) {
            agent.DmgBonus.Add(new(ModifierKey.Weapon(Id) + ModifierKey.CorePassive(), 0.2));
        }
    }
}
