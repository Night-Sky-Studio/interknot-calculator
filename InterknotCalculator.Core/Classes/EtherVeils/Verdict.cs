using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class Verdict : EtherVeil {
    // no-op
    public override void Enable(Agent agent) { }
    public override void Disable(Agent agent) { }
    public override string ToString() => nameof(Verdict);
}