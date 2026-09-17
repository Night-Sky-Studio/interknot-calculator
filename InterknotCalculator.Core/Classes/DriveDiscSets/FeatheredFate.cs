using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FeatheredFate : DriveDiscSet {
    public FeatheredFate() : base(DriveDiscSetId.FeatheredFate) {
        PartialBonus = [new(Affix.AnomalyProficiency, 30)];
        FullBonus = [new(Affix.AnomalyProficiency, 50)];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.MainAgent.AnomalyProficiency.Add(new(ModifierKey.DiscSet(Id, true), 
            0.15, ModifierType.Ratio));
    }
}