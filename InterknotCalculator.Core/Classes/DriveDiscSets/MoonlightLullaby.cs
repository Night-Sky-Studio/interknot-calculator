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
        
        ctx.Events.OnCalculationStarted += c => {
            if (c.Team[equipper].Speciality is not Speciality.Support) return;
            
            var key = ModifierKey.DiscSet(Id, true);
            if (!c.TryActivateGlobal(key)) return;
        
            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.Add(key, 0.18, ModifierType.CombatFlat);
            }
        };
    }
}
