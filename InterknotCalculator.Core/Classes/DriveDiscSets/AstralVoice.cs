using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class AstralVoice : DriveDiscSet {
    public AstralVoice() : base(DriveDiscSetId.AstralVoice) {
        PartialBonus = [new(Affix.AtkRatio, 0.1)];
        FullBonus = [new(Affix.DmgBonus, 0.24)];
    }
}
