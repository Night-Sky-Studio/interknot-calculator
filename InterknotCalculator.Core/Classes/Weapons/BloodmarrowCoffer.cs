using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class BloodmarrowCoffer : Weapon {
    public BloodmarrowCoffer() : base(WeaponId.BloodmarrowCoffer) {
        Speciality = Speciality.Armorer;
        Rarity = Rarity.A;
        MainStat = new(Affix.Def, 356);
        SecondaryStat = new(Affix.CritRate, 0.2);
        Passive = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        ctx.Events.OnCalculationStarted += c => {
            var agent = c.Team[equipper];

            agent.DmgBonus.Add(ModifierKey.Weapon(Id) + ModifierKey.Passive(),
                Math.Min(0.4, Math.Max(0, (agent.CritRate - 1) * 0.8)), ModifierType.CombatFlat);
        };
    }
}