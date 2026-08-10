using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class ColdBlooded : EtherVeil {
    public ColdBlooded() {
        BonusStats[Affix.CritDamage] += 0.05;
    }
}