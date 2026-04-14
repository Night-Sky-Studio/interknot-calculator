using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Koleda : Agent, IAgentReference<Koleda> {
    public static Koleda Reference() {
        var koleda = new Koleda();

        var kots = Resources.Current.GetDriveDiscSet(DriveDiscSetId.KingOfTheSummit);
        foreach (var fullBonus in kots.FullBonus) {
            koleda.ExternalBonus[fullBonus.Affix] += fullBonus.Value;
        }
        
        koleda.ApplyPassive();
        return koleda;
    }
    
    public Koleda() : base(AgentId.Koleda) {
        Speciality = Speciality.Stun;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.BelobogHeavyIndustries;

        Stats[Affix.Hp] = 8127;
        Stats[Affix.Atk] = 660 + 75;
        Stats[Affix.Def] = 594;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 116 + 18;
        Stats[Affix.AnomalyMastery] = 97;
        Stats[Affix.AnomalyProficiency] = 96;
        Stats[Affix.EnergyRegen] = 1.2;

        ExternalTagBonus.Add(new(Affix.DmgBonus, 0.35 * 2, tags: [SkillTag.Chain]));
    }

    // public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        // if (team.Count < 2) return [];
        //
        // if (team.Any(a => a.Element == Element) ||
        //     team.Any(a => a.Faction == Faction) ||
        //     team.Any(a => a.Speciality == Speciality.Rupture)) {
        //     return [
        //         new(Affix.DmgBonus, 0.35 * 2, tags: [SkillTag.Chain])
        //     ];
        // }
        //
        // return [];
    // }
}