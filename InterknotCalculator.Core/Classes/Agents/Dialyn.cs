using System.Runtime.InteropServices.Swift;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Dialyn : SupportAgent, IStunAgent {
    public Dialyn() : base(AgentId.Dialyn) {
        Speciality = Speciality.Stun;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.KrampusComplianceAuthority;
        
        Stats[Affix.Hp] = 8250;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 758;
        Stats[Affix.CritRate] = 0.194;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 110;
        Stats[Affix.AnomalyMastery] = 94;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;
    }

    public double EnemyStunBonusOverride { get; set; } = 0.3;

    public override void ApplyPassive() {
        // If her initial CRIT Rate surpasses 50%, her Impact increases
        // by 2 for each additional 1%, up to a maximum increase of 100.
        BonusStats[Affix.Impact] += Math.Min(100, Math.Max(0, CritRate - 0.5) * 2);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        // When another character in your squad is an Attack or Rupture character
        if (team.Any(a => a is { Speciality: Speciality.Attack or Speciality.Rupture })) {
            // Dialyn's EX Special Attack CRIT DMG is increased by 50%.
            TagBonus.Add(new(Affix.CritDamage, 0.5, tags: [SkillTag.ExSpecial]));

            // When an EX Special Attack or Ultimate is activated, all squad members gain the
            // Overwhelmingly Positive effect.
            // While Overwhelmingly Positive is active, DMG dealt is increased by 40% for 15s.
            ExternalBonus[Affix.DmgBonus] += 0.4;
        }
        
        return base.ApplyTeamPassive(team);
    }
}