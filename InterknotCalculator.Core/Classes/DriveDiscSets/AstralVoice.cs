using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class AstralVoice : DriveDiscSet {
    public AstralVoice() : base(DriveDiscSetId.AstralVoice) {
        PartialBonus = [new(Affix.AtkRatio, 0.1)];
        FullBonus = [];
    }

    // TODO(IKC-14): Track applications with context
    public override void ApplyPassive(Agent agent) {
        agent.BonusStats[Affix.DmgBonus] += 0.24;
        agent.ExternalBonus[Affix.DmgBonus] += 0.24;
    }
}
