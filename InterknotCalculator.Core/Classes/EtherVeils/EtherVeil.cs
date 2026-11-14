using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public abstract class EtherVeil {
    /// <summary>The modifiers this veil granted to each agent, toggled on every activation.</summary>
    private Dictionary<Agent, Modifier[]> Granted { get; } = new(ReferenceEqualityComparer.Instance);

    public void Activate(Context ctx) => SetActive(ctx, true);
    public void Deactivate(Context ctx) => SetActive(ctx, false);

    /// <summary>
    /// Adds this veil's modifiers to <paramref name="agent"/>. Called once per agent, on the first
    /// activation; later activations toggle the returned modifiers.
    /// </summary>
    protected abstract IEnumerable<Modifier> Initialize(Agent agent);

    private void SetActive(Context ctx, bool active) {
        foreach (var agent in ctx.Team.Values) {
            if (Granted.TryGetValue(agent, out var modifiers)) {
                foreach (var modifier in modifiers) modifier.Active = active;
            } else if (active) {
                Granted[agent] = Initialize(agent).ToArray();
            }
        }
    }

    public abstract new string ToString();
}
