using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class TheSkyAblaze : DriveDiscSet {
    public TheSkyAblaze() : base(DriveDiscSetId.TheSkyAblaze) {
        PartialBonus = [new(Affix.EtherDmgBonus, 0.1)];
        FullBonus = [];
    }

    public override void ApplyPassive(Agent agent) {
        if (!agent.Element.Matches(Element.Ether)) return;
                
        agent.BonusStats[Affix.CritDamage] += 0.3;
        agent.TagBonus.Add(new(Affix.CombatAtkRatio, 0.1, 
            tags: [SkillTag.ExSpecial, SkillTag.Ultimate]));
    }
}
