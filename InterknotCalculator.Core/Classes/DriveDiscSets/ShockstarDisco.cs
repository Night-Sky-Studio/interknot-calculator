using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ShockstarDisco : DriveDiscSet {
    public ShockstarDisco() : base(DriveDiscSetId.ShockstarDisco) {
        PartialBonus = [new(Affix.ImpactRatio, 0.06)];
        FullBonus = [new(Affix.DazeBonus, 0.2, tags: [SkillTag.BasicAtk, SkillTag.Dash, SkillTag.Counter])];
    }
}
