using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class Wellspring : EtherVeil {
    public Wellspring() {
        BonusStats[Affix.HpRatio] = new(ModifierKey.EtherVeil(nameof(Wellspring)), 0.05, ModifierType.Multiplicative);
    }
    
    public override string ToString() => nameof(Wellspring);
}