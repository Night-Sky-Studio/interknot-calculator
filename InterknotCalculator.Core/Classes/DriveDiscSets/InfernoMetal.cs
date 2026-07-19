using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class InfernoMetal : DriveDiscSet {
    public InfernoMetal() : base(DriveDiscSetId.InfernoMetal) {
        PartialBonus = [new(Affix.FireDmgBonus, 0.1)];
        // TODO(IKC-13): Needs context to track Burn anomaly on an enemy
        FullBonus = [new(Affix.CritRate, 0.28)];
    }
}
