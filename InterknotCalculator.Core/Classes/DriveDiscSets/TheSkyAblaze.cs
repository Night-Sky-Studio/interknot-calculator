using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class TheSkyAblaze : DriveDiscSet {
    public TheSkyAblaze() : base(DriveDiscSetId.TheSkyAblaze) {
        PartialBonus = [new(Affix.EtherDmgBonus, 0.1)];
        FullBonus = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var agent = ctx.Team[equipper];
        if (!agent.Element.Matches(Element.Ether)) return;

        agent.Stats[Affix.CritDamage] += new Modifier(ModifierKey.DiscSet(Id, true), 0.3);
        agent.Stats[Affix.CombatAtkRatio] += new Modifier(ModifierKey.DiscSet(Id, true), 0.1,
            ModifierType.Multiplicative, SkillTag.ExSpecial | SkillTag.Ultimate);
    }
}
