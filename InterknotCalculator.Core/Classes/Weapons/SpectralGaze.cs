using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class SpectralGaze : Weapon {
    public SpectralGaze() : base(WeaponId.SpectralGaze) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.CritRate, 0.24);
        Passive = [
            new(Affix.ImpactRatio, 0.2)
        ];
    }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            if (c.Team[equipper].Element.Matches(Element.Electric)) {
                c.Enemy.Defense.Subtract(new(ModifierKey.Weapon(Id) + ModifierKey.Passive(), 0.25, ModifierType.CombatRatio));
            }
        });
    }
}
