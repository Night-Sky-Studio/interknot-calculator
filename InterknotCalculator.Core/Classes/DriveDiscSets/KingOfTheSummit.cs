using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class KingOfTheSummit : DriveDiscSet {
    public KingOfTheSummit() : base(DriveDiscSetId.KingOfTheSummit) {
        PartialBonus = [new(Affix.DazeBonus, 0.06)];
        FullBonus = [];
    }

    public override void RegisterHooks(Context ctx, uint equipper = 0) {
        base.RegisterHooks(ctx, equipper);

        var agent = ctx.Team[equipper];
        if (agent is not { Speciality: Speciality.Stun }) return;

        var bonus = agent.CritRate >= 0.5 ? 0.3 : 0.15;
        
        var key = ModifierKey.DiscSet(Id, true);
        if (!ctx.TryActivateGlobal(key)) return;

        foreach (var a in ctx.Team.Values) {
            a.CritDamage.Add(new(key, bonus));
        }
    }
}
