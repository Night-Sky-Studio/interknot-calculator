using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class FlightOfFancy : Weapon {
    public FlightOfFancy() : base(WeaponId.FlightOfFancy) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 713);
        SecondaryStat = new(Affix.AnomalyProficiency, 90);
        Passive = [new(Affix.AnomalyBuildupBonus, 0.4)];
    }

    public override void ApplyPassive(Agent agent) {
        if (agent.Element is Element.Ether) {
            agent.BonusStats[Affix.AnomalyProficiency] += 20 * 6;
        }
    }
}
