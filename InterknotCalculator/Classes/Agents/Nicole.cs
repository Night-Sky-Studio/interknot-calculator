using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public class Nicole : Agent, IAgentReference<Nicole> {
    public Nicole() : base(1031) {
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
    public static Nicole Reference() {
        var nicole = new Nicole {
            Stats = {
                [Affix.Hp] = 10000,
                [Affix.Atk] = 2400,
                [Affix.Def] = 800,
                [Affix.AnomalyProficiency] = 320
            }
        };
        
        var theVault = Resources.Current.GetWeapon(13103);
        foreach (var stat in theVault.ExternalBonus) {
            nicole.ExternalBonus[stat.Affix] += stat.Value;
        }

        var astralVoice = Resources.Current.GetDriveDiscSet(32800);

        var qaBonus = astralVoice.FullBonus.First();
        nicole.ExternalBonus[qaBonus.Affix] += qaBonus.Value;

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
            return [new(0.25, Affix.EtherDmgBonus)];
        }
        
        return [];
    }
}