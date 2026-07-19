using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class BunnyInWonderland : DriveDiscSet {
    public BunnyInWonderland() : base(DriveDiscSetId.BunnyInWonderland) {
        PartialBonus = [new(Affix.HpRatio, 0.1)];
        FullBonus = [];
    }

    public override void ApplyPassive(Agent agent) {
        if (agent.Speciality is Speciality.Defense) {
            agent.ExternalBonus[Affix.DmgBonus] += 0.18;
        }
    }
}
