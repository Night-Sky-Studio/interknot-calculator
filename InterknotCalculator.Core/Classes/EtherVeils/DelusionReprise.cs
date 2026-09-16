using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class DelusionReprise : EtherVeil {
    public DelusionReprise() {
        BonusStats[Affix.Atk] = new(ModifierKey.EtherVeil(nameof(DelusionReprise)), 50);
    }

    public override string ToString() => nameof(DelusionReprise);
}