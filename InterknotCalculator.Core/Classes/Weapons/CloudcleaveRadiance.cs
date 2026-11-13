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
        
        var etherVeilPassiveKey = ModifierKey.Weapon(Id) + ModifierKey.Passive() + ModifierKey.EtherVeil();
        
        // Created on the first activation, toggled after that
        Modifier[]? buffs = null;
        
        ctx.Events.OnEtherVeilActivated += (c, _) => {
            if (buffs is null) {
                buffs = [
                    c.MainAgent.DmgBonus.Add(etherVeilPassiveKey, 0.25, ModifierType.CombatFlat),
                    c.MainAgent.CritDamage.Add(etherVeilPassiveKey, 0.25, ModifierType.CombatFlat)
                ];
            } else {
                foreach (var buff in buffs) buff.Active = true;
            }
        };
        
        ctx.Events.OnEtherVeilDeactivated += (_, _) => {
            if (buffs is null) return;
            foreach (var buff in buffs) buff.Active = false;
        };
    }
}