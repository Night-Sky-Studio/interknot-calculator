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
        
        ctx.Events.OnCalculationStarted.Add(c => {
            var key = ModifierKey.Weapon(Id) + ModifierKey.Passive();
        
            if (!c.TryActivateGlobal(key)) return;
        
            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.Add(new(key, 0.202, ModifierType.CombatFlat));
            }
        });
    }
}
