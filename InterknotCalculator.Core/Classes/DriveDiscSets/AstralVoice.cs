using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class AstralVoice : DriveDiscSet {
    public AstralVoice() : base(DriveDiscSetId.AstralVoice) {
        PartialBonus = [new(Affix.AtkRatio, 0.1)];
        FullBonus = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var key = ModifierKey.DiscSet(Id, true);
        if (!ctx.TryActivateGlobal(key)) return;
        
        foreach (var agent in ctx.Team.Values) {
            agent.Stats[Affix.DmgBonus] += new Modifier(key, 0.24, ModifierType.Multiplicative);
        }
    }
}
