using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class CloudcleaveRadiance : Weapon {
    public CloudcleaveRadiance() : base(WeaponId.CloudcleaveRadiance) {
        Speciality = Speciality.Attack;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 743);
        SecondaryStat = new(Affix.CritDamage, 0.48);
        Passive = [new(Affix.PhysicalResPen, 0.2)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var etherVeilPassiveKey = ModifierKey.Weapon(Id) + ModifierKey.CorePassive() + ModifierKey.EtherVeil("Any");
        
        ctx.Events.OnEtherVeilActivated.Add((c, _) => {
            if (c.MainAgent.DmgBonus.Contains(etherVeilPassiveKey)) return;
            c.MainAgent.DmgBonus.AddUnique(new(etherVeilPassiveKey, 0.25));
            c.MainAgent.CritDamage.AddUnique(new(etherVeilPassiveKey, 0.25));
        });
        
        ctx.Events.OnEtherVeilDeactivated.Add((c, e) => {
            c.MainAgent.DmgBonus.RemoveKey(etherVeilPassiveKey);
            c.MainAgent.CritDamage.RemoveKey(etherVeilPassiveKey);
        });
    }
}