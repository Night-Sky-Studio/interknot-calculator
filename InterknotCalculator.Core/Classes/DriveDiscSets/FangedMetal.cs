using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FangedMetal : DriveDiscSet {
    // repeated triggers reset duration
    public FangedMetal() : base(DriveDiscSetId.FangedMetal) {
        PartialBonus = [new(Affix.PhysicalDmgBonus, 0.1)];
        FullBonus = [];
    }

    private bool IsActive { get; set; }
    
    void Activate(Context c, uint equipper) {
        if (IsActive) return;
        IsActive = true;
        c.Team[equipper].DmgBonus.Add(new(ModifierKey.DiscSet(Id, true), 0.35, ModifierType.CombatFlat));
    }
    
    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            // If calculation parameters specified that enemy has Assault
            if (c.Enemy.AfflictedAnomaly?.Element.Matches(Element.Physical) is not true) return;
            Activate(c, equipper);
        });

        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            if (!e.Element.Matches(Element.Physical)) return;
            Activate(c, equipper);
        });
    }
}
