using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class HalfSugarBunny : Weapon {
    public HalfSugarBunny() : base(WeaponId.HalfSugarBunny) {
        Speciality = Speciality.Defense;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.HpRatio, 0.3);
        ExternalBonus = [
            new(Affix.AtkRatio, 0.1),
            new(Affix.HpRatio, 0.1)
        ];
    }

    private bool EtherVeilBonusActive { get; set; } = false;
    
    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnEtherVeilActivated.Add((c, veil) => {
            if (EtherVeilBonusActive) return;

            foreach (var (_, agent) in ctx.Team) {
                agent.BonusStats[Affix.CritDamage] += 0.3;
            }
            
            EtherVeilBonusActive = true;
        });
    }
}