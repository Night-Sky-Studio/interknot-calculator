using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class ColdBlooded : EtherVeil {
    public ColdBlooded() {
        BonusStats[Affix.CritDamage] = new(ModifierKey.EtherVeil(nameof(ColdBlooded)), 0.05);
    }

    public override string ToString() => nameof(ColdBlooded);
}