using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public sealed class Lycaon : Agent, IStunAgent, ISupportAgent<Lycaon> {
    public static Lycaon Reference() {
        var lycaon = new Lycaon();

        lycaon.ApplyPassive();
        
        return lycaon;
    }

    public Lycaon() : base(1141) {
        Speciality = Speciality.Stun;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.VictoriaHousekeeping;
        
        Stats[Affix.Hp] = 8416;
        Stats[Affix.Def] = 606;
        Stats[Affix.Atk] = 653 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 119 + 18;
        Stats[Affix.AnomalyMastery] = 90;
        Stats[Affix.AnomalyProficiency] = 91;
        Stats[Affix.EnergyRegen] = 1.2; 
    }

    public override void ApplyPassive() {
        BonusStats[Affix.DazeBonus] = 0.8;
        ExternalBonus[Affix.IceResPen] = 0.25;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];
        
        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            EnemyStunBonusOverride = 0.35;
        }

        return [];
    }

    public double EnemyStunBonusOverride { get; set; }
}