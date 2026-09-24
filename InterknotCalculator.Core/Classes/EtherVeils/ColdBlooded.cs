using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class ColdBlooded : EtherVeil {
    private ModifierKey Key { get; } = ModifierKey.EtherVeil(nameof(ColdBlooded));
    
    public override void Enable(Agent agent) {
        agent.CritDamage.AddUnique(new(Key, 0.05));
    }
    public override void Disable(Agent agent) {
        agent.CritDamage.RemoveKey(Key);
    }

    public override string ToString() => nameof(ColdBlooded);
}