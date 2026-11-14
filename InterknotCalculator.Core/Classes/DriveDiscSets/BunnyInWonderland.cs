using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class BunnyInWonderland : DriveDiscSet {
    public BunnyInWonderland() : base(DriveDiscSetId.BunnyInWonderland) {
        PartialBonus = [new(Affix.HpRatio, 0.1)];
        FullBonus = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);
        
        ctx.Events.OnCalculationStarted += c => {
            if (c.Team[equipper].Speciality is not Speciality.Defense) return;
            
            var key = ModifierKey.DiscSet(Id, true);
            if (!c.TryActivateGlobal(key)) return;
            
            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.Add(key, 0.18, ModifierType.CombatFlat);
            }
        };
    }
}
