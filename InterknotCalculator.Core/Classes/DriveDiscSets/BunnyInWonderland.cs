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
        
        foreach (var agent in ctx.Team.Values) {
            if (agent.Speciality is Speciality.Defense) {
                agent.Stats[Affix.DmgBonus] += new Modifier(ModifierKey.DiscSet(Id, true), 0.18, ModifierType.Multiplicative);
            }
        }
    }
}
