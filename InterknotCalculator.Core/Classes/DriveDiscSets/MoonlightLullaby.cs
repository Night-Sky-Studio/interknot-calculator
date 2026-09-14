using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class MoonlightLullaby : DriveDiscSet {
    public MoonlightLullaby() : base(DriveDiscSetId.MoonlightLullaby) {
        PartialBonus = [new(Affix.EnergyRegenRatio, 0.2)];
        FullBonus = [];
    }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        var agent = ctx.Team[equipper];
        if (agent is not { Speciality: Speciality.Support }) return;
        
        var key = ModifierKey.DiscSet(Id, true);
        if (!ctx.TryActivateGlobal(key)) return;
        
        foreach (var a in ctx.Team.Values) {
            a.Stats[Affix.DmgBonus] += new Modifier(key, 0.18, ModifierType.Multiplicative);
        }
    }
}
