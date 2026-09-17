using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class Timeweaver : Weapon {
    public Timeweaver() : base(WeaponId.Timeweaver) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AtkRatio, 0.3);
        Passive = [
            new(Affix.AnomalyBuildupBonus, 0.3),
            new(Affix.AnomalyProficiency, 75)
        ];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        var agent = ctx.Team[equipper];
        
        agent.DisorderDmgBonus.Add(new(ModifierKey.Weapon(Id) + ModifierKey.Passive(), 
            agent.AnomalyProficiency > 375 ? 0.25 : 0));
    }
}
