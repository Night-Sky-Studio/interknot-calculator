using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class Verdict : EtherVeil {
    // no-op
    protected override IEnumerable<Modifier> Initialize(Agent agent) => [];
    public override string ToString() => nameof(Verdict);
}