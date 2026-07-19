using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class DawnsBloom : DriveDiscSet {
    public DawnsBloom() : base(DriveDiscSetId.DawnsBloom) {
        PartialBonus = [new(Affix.DmgBonus, 0.15, tags: [SkillTag.BasicAtk])];
        FullBonus = [];
    }
    
    public override void ApplyPassive(Agent agent) {
        agent.TagBonus.Add(new(Affix.DmgBonus, 
            agent.Speciality is Speciality.Attack ? 0.4 : 0.2, 
            tags: [SkillTag.BasicAtk]));
    }
}
