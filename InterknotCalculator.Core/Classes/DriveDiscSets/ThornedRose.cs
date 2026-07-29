using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ThornedRose : DriveDiscSet {
    public ThornedRose() : base(DriveDiscSetId.ThornedRose) {
        PartialBonus = [new(Affix.DefRatio, 0.16)];
        FullBonus = [new(Affix.DmgBonus, 0.15)];
    }

    public override void ApplyPassive(Agent agent) {
        if (agent.Def >= 1000) {
            agent.BonusStats[Affix.CritRate] += 0.08;
        }
        if (agent.Def >= 1800) {
            agent.BonusStats[Affix.CritRate] += 0.08;
        }
    }
}