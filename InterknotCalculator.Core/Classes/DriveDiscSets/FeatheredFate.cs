using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class FeatheredFate : DriveDiscSet {
    public FeatheredFate() : base(DriveDiscSetId.FeatheredFate) {
        PartialBonus = [new(Affix.AnomalyProficiency, 30)];
        FullBonus = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted.Add(c => {
            var agent = c.Team[equipper];
            
            agent.AnomalyProficiency.Add(new(ModifierKey.DiscSet(Id, true), 
                50, ModifierType.CombatFlat));

            if (agent.Element.Matches(Element.Lumiflux)) {
                agent.AnomalyDmgBonus.Add(new(ModifierKey.DiscSet(Id, true),
                    0.15, ModifierType.CombatFlat));
            }
        });
        
    }
}