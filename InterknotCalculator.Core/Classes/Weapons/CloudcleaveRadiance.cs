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

    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnEtherVeilActivated.Add((c, e) => {
            c.MainAgent.BonusStats[Affix.DmgBonus] += 0.25;
            c.MainAgent.BonusStats[Affix.CritDamage] += 0.25;
        });
        
        ctx.Events.OnEtherVeilDeactivated.Add((c, e) => {
            c.MainAgent.BonusStats[Affix.DmgBonus] -= 0.25;
            c.MainAgent.BonusStats[Affix.CritDamage] -= 0.25;
        });
    }
}