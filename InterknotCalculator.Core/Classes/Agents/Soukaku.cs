using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Soukaku : SupportAgent, IAgentReference<Soukaku> {
    public Soukaku() : base(AgentId.Soukaku) {
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

    public static Soukaku Reference(uint weaponId, uint setId) {
        var soukaku = new Soukaku {
            Stats = {
                [Affix.Atk] = 2500
            }
        };

        soukaku.SetWeaponPassive(weaponId);
        soukaku.SetDriveDiscsPassive(setId);
        
        soukaku.ApplyPassive();
        
        return soukaku;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.Atk] += Math.Min(InitialAtk * 2 * 0.2, 1000);
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Element == Element) ||
            team.Any(a => a.Faction == Faction)) {
            return [new(Affix.IceDmgBonus, 0.2)];
        }
        
        return [];
    }
}