using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class Rina : SupportAgent, IAgentReference<Rina> {
    public static Rina Reference(uint weaponId, uint setId) {
        var rina = new Rina {
            Stats = {
                [Affix.Atk] = 2600
            },
            BonusStats = {
                [Affix.PenRatio] = 0.3
            }
        };

        rina.SetWeaponPassive(weaponId);
        rina.SetDriveDiscsPassive(setId);
        
        rina.ApplyPassive();
        
        return rina;
    }
    public Rina() : base(AgentId.Rina) {
        Speciality = Speciality.Support;
        Element = Element.Electric;
        Rarity = Rarity.S;
        Faction = Faction.VictoriaHousekeeping;
        
        Stats[Affix.Hp] = 8609;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 642 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 83;
        Stats[Affix.AnomalyMastery] = 92;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;

        Stats[Affix.PenRatio] = 0.144;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.PenRatio] += Math.Min(PenRatio * 0.25 + 0.12, 0.3);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(Affix.ElectricDmgBonus, 0.1)];
        }
        
        return [];
    }
}