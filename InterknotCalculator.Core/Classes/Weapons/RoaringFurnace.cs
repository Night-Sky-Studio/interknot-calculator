using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class RoaringFurnace : Weapon {
    public RoaringFurnace() : base(WeaponId.RoaringFurnace) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        Passive = [new(Affix.DazeBonus, 0.28, tags: SkillTag.ExSpecial | SkillTag.Chain | SkillTag.Ultimate)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        var key = ModifierKey.Weapon(Id) + ModifierKey.CorePassive();

        if (!ctx.TryActivateGlobal(key)) return;

        foreach (var agent in ctx.Team.Values) {
            agent.DmgBonus.Add(new(key, 0.2));
        }
    }
}
