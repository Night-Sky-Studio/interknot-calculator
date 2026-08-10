using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public abstract class EtherVeil {
    public SafeDictionary<Affix, double> BonusStats { get; set; } = new();

    public void Activate(Context ctx) {
        foreach (var (_, agent) in ctx.Team) {
            foreach (var (affix, bonus) in BonusStats) {
                agent.BonusStats[affix] += bonus;
            }
        }
    }
    
    public void Deactivate(Context ctx) {
        foreach (var (_, agent) in ctx.Team) {
            foreach (var (affix, bonus) in BonusStats) {
                agent.BonusStats[affix] -= bonus;
            }
        }
    }
}