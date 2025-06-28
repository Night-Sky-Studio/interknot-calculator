using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public class Soukaku : Agent, IAgentReference<Soukaku> {
    public Soukaku() : base(1131) {
        Speciality = Speciality.Support;
        Element = Element.Ice;
        Rarity = Rarity.A;
        Faction = Faction.HollowSpecialOperationsSection6;
        
        Stats[Affix.Hp] = 8026;
        Stats[Affix.Def] = 597;
        Stats[Affix.Atk] = 590 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 96;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.56;
    }

    public static Soukaku Reference() {
        var soukaku = new Soukaku {
            Stats = {
                [Affix.Atk] = 2500
            }
        };

        var bashfulDemon = Resources.Current.GetWeapon(13113);
        
        foreach (var stat in bashfulDemon.ExternalBonus) {
            soukaku.ExternalBonus[stat.Affix] += stat.Value;
        }

        var astralVoice = Resources.Current.GetDriveDiscSet(32800);

        var qaBonus = astralVoice.FullBonus.First();
        soukaku.ExternalBonus[qaBonus.Affix] += qaBonus.Value;

        soukaku.ApplyPassive();
        
        return soukaku;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.Atk] += Math.Min(Atk * 2 * 0.2, 1000);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(0.2, Affix.IceDmgBonus)];
        }
        
        return [];
    }
}