using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class InfernoMetal : DriveDiscSet {
    public InfernoMetal() : base(DriveDiscSetId.InfernoMetal) {
        PartialBonus = [new(Affix.FireDmgBonus, 0.1)];
        FullBonus = [new(Affix.CritRate, 0.28)];
    }

    private bool IsActive { get; set; }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            if (e is not { Element: Element.Fire } || IsActive) return;

            IsActive = true;
            c.Team[equipper].Stats[Affix.CritRate] += new Modifier(ModifierKey.DiscSet(Id, true), 0.28);
        });
    }
}
