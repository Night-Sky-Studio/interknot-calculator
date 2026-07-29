using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FeatheredFate : DriveDiscSet {
    public FeatheredFate() : base(DriveDiscSetId.FeatheredFate) {
        PartialBonus = [new(Affix.AnomalyProficiency, 30)];
        FullBonus = [new(Affix.AnomalyProficiency, 50)];
    }

    public override void ApplyPassive(Agent agent) {
        if (agent.Element.Matches(Element.Lumiflux)) {
            agent.BonusStats[Affix.AnomalyDmgBonus] += 0.15;
        }
    }
}