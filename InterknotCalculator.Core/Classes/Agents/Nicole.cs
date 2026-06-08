using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Nicole : SupportAgent, IAgentReference<Nicole> {
    public Nicole() : base(AgentId.Nicole) {
        Speciality = Speciality.Support;
        Element = Element.Ether;
        Rarity = Rarity.A;
        Faction = Faction.CunningHares;
        
        Stats[Affix.Hp] = 8145;
        Stats[Affix.Def] = 622;
        Stats[Affix.Atk] = 574 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 88;
        Stats[Affix.AnomalyMastery] = 93;
        Stats[Affix.AnomalyProficiency] = 90;
        Stats[Affix.EnergyRegen] = 1.56;
    }
    public static Nicole Reference(uint weaponId, uint setId) {
        var nicole = new Nicole {
            Stats = {
                [Affix.Hp] = 10000,
                [Affix.Atk] = 2400,
                [Affix.Def] = 800,
                [Affix.AnomalyProficiency] = 320
            }
        };
        
        nicole.SetWeaponPassive(weaponId);
        nicole.SetDriveDiscsPassive(setId);
        
        nicole.ApplyPassive();
        
        return nicole;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.ResPen] += 0.4;
        // M6
        ExternalBonus[Affix.CritRate] += 0.15;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(Affix.EtherDmgBonus, 0.25)];
        }
        
        return [];
    }
}