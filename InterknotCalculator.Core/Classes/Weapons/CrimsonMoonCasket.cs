using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class CrimsonMoonCasket : Weapon {
    public CrimsonMoonCasket() : base(WeaponId.CrimsonMoonCasket) {
        Speciality = Speciality.Stun;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.EnergyRegenRatio, 0.6);
        Passive = [
            new(Affix.CritRate, 0.24),
            new(Affix.WindResPen, 0.15)
        ];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        ctx.Events.OnCalculationStarted += c => {
            var agent = c.Team[equipper];
            if (agent.Element.Matches(Element.Wind)) {
                agent.DazeBonus.Add(ModifierKey.Weapon(Id) + ModifierKey.Passive(), 
                    0.16, ModifierType.CombatFlat);

                foreach (var a in c.Team.Values) {
                    if (a.Id == equipper) continue;
                    
                    a.DmgBonus.Add(ModifierKey.Weapon(Id) + ModifierKey.Passive(), 
                        0.2, ModifierType.CombatFlat);
                }
            }
        };
    }
}