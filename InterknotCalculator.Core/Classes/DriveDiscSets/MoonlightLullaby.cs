using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class MoonlightLullaby : DriveDiscSet {
    public MoonlightLullaby() : base(DriveDiscSetId.MoonlightLullaby) {
        PartialBonus = [new(Affix.EnergyRegenRatio, 0.2)];
        FullBonus = [];
    }

    // TODO(IKC-14): Track applications with context
    public override void ApplyPassive(Agent agent) {
        if (agent.Speciality is not Speciality.Support) return;
        
        agent.BonusStats[Affix.DmgBonus] += 0.18;
        agent.ExternalBonus[Affix.DmgBonus] += 0.18;
    }
}
