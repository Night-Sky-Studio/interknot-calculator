using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FangedMetal : DriveDiscSet {
    // repeated triggers reset duration
    public FangedMetal() : base(DriveDiscSetId.FangedMetal) {
        PartialBonus = [new(Affix.PhysicalDmgBonus, 0.1)];
        FullBonus = [new(Affix.DmgBonus, 0.35)];
    }

    private bool IsActive { get; set; }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            if (e is not { Element: Element.Physical } || IsActive) return;
            IsActive = true;
            c.Team[equipper].DmgBonus.Add(new(ModifierKey.DiscSet(Id, true), 0.35, ModifierType.Ratio));
        });
    }
}
