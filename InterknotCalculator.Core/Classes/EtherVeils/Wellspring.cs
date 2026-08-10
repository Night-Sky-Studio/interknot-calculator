using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class Wellspring : EtherVeil {
    public Wellspring() {
        BonusStats[Affix.HpRatio] += 0.05;
    }
}