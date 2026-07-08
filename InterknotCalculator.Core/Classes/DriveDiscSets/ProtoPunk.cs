using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ProtoPunk : DriveDiscSet {
    public ProtoPunk() : base(DriveDiscSetId.ProtoPunk) {
        PartialBonus = [];
        FullBonus = [new(Affix.DmgBonus, 0.15, tags: [SkillTag.DefensiveAssist, SkillTag.EvasiveAssist])];
    }
}
