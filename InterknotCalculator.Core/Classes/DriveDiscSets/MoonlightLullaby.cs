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
        
        ctx.Events.OnCalculationStarted.Add(c => {
            var agent = c.Team[equipper];
            if (agent is not { Speciality: Speciality.Support }) return;
        
            var key = ModifierKey.DiscSet(Id, true);
            if (!c.TryActivateGlobal(key)) return;
        
            foreach (var a in c.Team.Values) {
                a.DmgBonus.Add(new(key, 0.18, ModifierType.CombatFlat));
            }
        });
    }
}
