using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class MoonlightLullaby : DriveDiscSet {
    public MoonlightLullaby() : base(DriveDiscSetId.MoonlightLullaby) {
        PartialBonus = [new(Affix.EnergyRegenRatio, 0.2)];
        FullBonus = [];
    }
}
