using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class FlightOfFancy : Weapon {
    public FlightOfFancy() : base(WeaponId.FlightOfFancy) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AnomalyProficiency, 90);
        Passive = [new(Affix.AnomalyBuildupBonus, 0.4)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        ctx.Events.OnCalculationStarted.Add(c => {
            var agent = c.Team[equipper];
            if (!agent.Element.Matches(Element.Ether)) return;
            
            agent.AnomalyProficiency.Add(new(ModifierKey.Weapon(Id) + ModifierKey.Passive(),
                20 * 6, ModifierType.CombatFlat));
        });
    }
}
