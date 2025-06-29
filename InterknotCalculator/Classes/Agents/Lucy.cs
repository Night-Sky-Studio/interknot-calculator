using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public class Lucy : Agent, IAgentReference<Lucy> {
    public Lucy() : base(1151) {
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

    public static Lucy Reference() {
        var lucy = new Lucy {
            Stats = {
                [Affix.Atk] = 1932
            }
        };
        
        var kaboomTheCannon = Resources.Current.GetWeapon(13115);
        foreach (var stat in kaboomTheCannon.ExternalBonus) {
            lucy.ExternalBonus[stat.Affix] += stat.Value;
        }

        var astralVoice = Resources.Current.GetDriveDiscSet(32800);

        var qaBonus = astralVoice.FullBonus.First();
        lucy.ExternalBonus[qaBonus.Affix] += qaBonus.Value;
        
        lucy.ApplyPassive();

        return lucy;
    }

    public override void ApplyPassive() {
        // M6
        ExternalBonus[Affix.Atk] += Math.Min(Atk * 0.258 + 104, 600);
        // M4
        ExternalBonus[Affix.CritDamage] += 0.1;
    }

}