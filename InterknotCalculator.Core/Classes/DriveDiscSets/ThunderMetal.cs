using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class ThunderMetal : DriveDiscSet {
    public ThunderMetal() : base(DriveDiscSetId.ThunderMetal) {
        PartialBonus = [new(Affix.ElectricDmgBonus, 0.1)];
        FullBonus = [];
    }
    
    private bool IsActive { get; set; }

    void Activate(Context c, uint equipper) {
        if (IsActive) return;
        IsActive = true;
        c.Team[equipper].Atk.Add(new(ModifierKey.DiscSet(Id, true), 0.28, ModifierType.CombatFlat));
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            if (c.Enemy.AfflictedAnomaly?.Element.Matches(Element.Electric) is not true) return;
            Activate(c, equipper);
        });
        
        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            if (!e.Element.Matches(Element.Electric)) return;
            Activate(c, equipper);
        });
    }
}
