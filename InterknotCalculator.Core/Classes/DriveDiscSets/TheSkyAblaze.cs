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
        
        ctx.Events.OnCalculationStarted.Add(c => {
            var agent = c.Team[equipper];
            if (!agent.Element.Matches(Element.Ether)) return;

            var key = ModifierKey.DiscSet(Id, true);
        
            agent.CritDamage.Add(new(key, 0.3, ModifierType.CombatFlat));
            agent.Atk.Add(new(key, 0.1,
                ModifierType.CombatRatio, SkillTag.ExSpecial | SkillTag.Ultimate));
        });
    }
}
