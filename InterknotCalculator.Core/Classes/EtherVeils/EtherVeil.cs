using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public abstract class EtherVeil {
    public Dictionary<Affix, Modifier> BonusStats { get; } = new();

    public void Activate(Context ctx) {
        foreach (var agent in ctx.Team.Values) {
            foreach (var (affix, bonus) in BonusStats) {
                agent.Stats[affix] += bonus;
            }
        }
    }
    
    public void Deactivate(Context ctx) {
        foreach (var agent in ctx.Team.Values) {
            foreach (var bonus in BonusStats.Values) {
                agent.Stats.RemoveAllModifiers(bonus.Key);
            }
        }
    }

    public abstract new string ToString();
}