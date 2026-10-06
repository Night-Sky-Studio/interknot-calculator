using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class TremorTrigramVessel : Weapon {
    public TremorTrigramVessel() : base(WeaponId.TremorTrigramVessel) {
        Speciality = Speciality.Defense;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 624);
        SecondaryStat = new(Affix.AtkRatio, 0.5);
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            var key = ModifierKey.Weapon(Id) + ModifierKey.Passive();

            if (!c.TryActivateGlobal(key)) return;

            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.Add(new(key, 0.4, ModifierType.CombatFlat, SkillTag.ExSpecial | SkillTag.Ultimate));
            }
        });
    }
}
