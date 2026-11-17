using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class CrimsonThirst : Weapon {
    public CrimsonThirst() : base(WeaponId.CrimsonThirst) {
        Speciality = Speciality.Armorer;
        Rarity = Rarity.S;
        MainStat = new(Affix.Def, 431);
        SecondaryStat = new(Affix.DefRatio, 0.48);
        Passive = [
            new(Affix.CritRate, 0.25),
            new(Affix.ElectricDmgBonus, 0.15)
        ];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        // After the equipper uses an EX Special Attack or triggers Maim,
        // their Electric Sharp DMG increases by 10% for 40s
        Modifier? sharpBonus = null;

        ctx.Events.OnCalculationStarted += c => {
            sharpBonus = c.Team[equipper].GetStat(Affix.ElectricSharpDmgBonus)
                .Add(ModifierKey.Weapon(Id) + ModifierKey.Passive(), 0.1, ModifierType.CombatFlat, active: false);
        };

        ctx.Events.OnActionExecuted += (_, e) => {
            if (sharpBonus is null || e.Agent.Id != equipper || e.Ability.Tag is not SkillTag.ExSpecial) return;
            sharpBonus.Active = true;
        };

        ctx.Events.OnMaim += (_, e) => {
            if (sharpBonus is null || e.Agent.Id != equipper) return;
            sharpBonus.Active = true;
        };
    }
}
