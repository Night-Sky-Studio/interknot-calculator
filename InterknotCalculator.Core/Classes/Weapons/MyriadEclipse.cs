using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class MyriadEclipse : Weapon {
    public MyriadEclipse() : base(WeaponId.MyriadEclipse) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.CritDamage, 0.45)
        ];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        ctx.Events.OnCalculationStarted.Add(c => {
            // When the equipper's EX Special, Chain Attack or Ultimate deals Ice DMG,
            // the equipper's attacks ignore 25% of the target's DEF (Weapon_Ability_14129)
            var agent = c.Team[equipper];
            if (!agent.Element.Matches(Element.Ice)) return;

            agent.DefPen.Add(new(ModifierKey.Weapon(Id) + ModifierKey.Passive(),
                0.25, ModifierType.CombatFlat,
                tags: SkillTag.ExSpecial | SkillTag.Chain | SkillTag.Ultimate));
        });
    }
}
