using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class InfernoMetal : DriveDiscSet {
    public InfernoMetal() : base(DriveDiscSetId.InfernoMetal) {
        PartialBonus = [new(Affix.FireDmgBonus, 0.1)];
        FullBonus = [new(Affix.CritRate, 0.28)];
    }
}
