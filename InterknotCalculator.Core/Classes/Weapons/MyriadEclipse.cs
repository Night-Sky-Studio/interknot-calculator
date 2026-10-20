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
            c.Enemy.Defense.Subtract(new(ModifierKey.Weapon(Id) + ModifierKey.Passive(), 
                0.25, ModifierType.CombatRatio, 
                tags: SkillTag.ExSpecial | SkillTag.Chain | SkillTag.Ultimate));
        });
    }
}
