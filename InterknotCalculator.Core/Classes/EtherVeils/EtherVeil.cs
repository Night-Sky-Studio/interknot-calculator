using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public abstract class EtherVeil {
    public void Activate(Context ctx) {
        foreach (var agent in ctx.Team.Values) {
            Enable(agent);
        }
    }
    
    public void Deactivate(Context ctx) {
        foreach (var agent in ctx.Team.Values) {
            Disable(agent);
        }
    }

    public abstract void Enable(Agent agent);
    public abstract void Disable(Agent agent);

    public abstract new string ToString();
}