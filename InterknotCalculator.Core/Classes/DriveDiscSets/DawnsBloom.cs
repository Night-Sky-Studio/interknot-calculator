using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class DawnsBloom : DriveDiscSet {
    public DawnsBloom() : base(DriveDiscSetId.DawnsBloom) {
        PartialBonus = [new(Affix.DmgBonus, 0.15, tags: [SkillTag.BasicAtk])];
        FullBonus = [];
    }
}
