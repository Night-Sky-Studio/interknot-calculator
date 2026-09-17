using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class Metanukimorphosis : Weapon {
    public Metanukimorphosis() : base(WeaponId.Metanukimorphosis) {
        Speciality = Speciality.Support;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.6);
        Passive = [new(Affix.AnomalyMastery, 30)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var key = ModifierKey.Weapon(Id) + ModifierKey.Passive();
        
        if (!ctx.TryActivateGlobal(key)) return;

        foreach (var agent in ctx.Team.Values) {
            agent.AnomalyProficiency.Add(new(key, 60, ModifierType.CombatFlat));
        }
    }
}
