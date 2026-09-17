
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ThornedRose : DriveDiscSet {
    public ThornedRose() : base(DriveDiscSetId.ThornedRose) {
        PartialBonus = [new(Affix.DefRatio, 0.16)];
        FullBonus = [new(Affix.DmgBonus, 0.15)];
    }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var agent = ctx.Team[equipper];

        if (agent.Def >= 1000) {
            agent.CritRate.Add(new(ModifierKey.DiscSet(Id, true), 0.08));
        }
        if (agent.Def >= 1800) {
            agent.CritRate.Add(new(ModifierKey.DiscSet(Id, true), 0.08));
        }
    }
}