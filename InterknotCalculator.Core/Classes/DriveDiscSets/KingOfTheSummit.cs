using System.IO.Compression;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class KingOfTheSummit : DriveDiscSet {
    public KingOfTheSummit() : base(DriveDiscSetId.KingOfTheSummit) {
        PartialBonus = [new(Affix.DazeBonus, 0.06)];
        FullBonus = [];
    }

    // TODO(IKC-14): Track applications with context
    public override void ApplyPassive(Agent agent) {
        if (agent.Speciality is not Speciality.Stun) return;

        var bonus = agent.CritRate >= 0.5 ? 0.3 : 0.15;
        
        agent.BonusStats[Affix.CritDamage] += bonus;
        agent.ExternalBonus[Affix.CritDamage] += bonus;
    }
}
