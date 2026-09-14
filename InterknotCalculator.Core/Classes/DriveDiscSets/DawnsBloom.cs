using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class DawnsBloom : DriveDiscSet {
    public DawnsBloom() : base(DriveDiscSetId.DawnsBloom) {
        PartialBonus = [new(Affix.DmgBonus, 0.15, tags: SkillTag.BasicAtk)];
        FullBonus = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        var agent = ctx.Team[equipper];
        agent.Stats[Affix.DmgBonus] += new Modifier(ModifierKey.DiscSet(Id, true),
            agent.Speciality is Speciality.Attack ? 0.4 : 0.2,
            ModifierType.Multiplicative, SkillTag.BasicAtk);
    }
}
