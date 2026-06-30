using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Lucy : SupportAgent, IAgentReference<Lucy> {
    public Lucy() : base(AgentId.Lucy) {
        Speciality = Speciality.Support;
        Element = Element.Fire;
        Rarity = Rarity.A;
        Faction = Faction.SonsOfCalydon;

        Stats[Affix.Hp] = 8025;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 583 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 93;
        Stats[Affix.AnomalyProficiency] = 94;
        Stats[Affix.EnergyRegen] = 1.56;
    }

    public static Lucy Reference(uint weaponId, uint setId) {
        var lucy = new Lucy {
            Stats = {
                [Affix.Atk] = 1932
            }
        };
        
        lucy.SetWeaponPassive(weaponId);
        lucy.SetDriveDiscsPassive(setId);
        
        lucy.ApplyPassive();

        return lucy;
    }

    public override void ApplyPassive() {
        // M6
        ExternalBonus[Affix.Atk] += Math.Min(InitialAtk * 0.258 + 104, 600);
        // M4
        ExternalBonus[Affix.CritDamage] += 0.1;
    }
}