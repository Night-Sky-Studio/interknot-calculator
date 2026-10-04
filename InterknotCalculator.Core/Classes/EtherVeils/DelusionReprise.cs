using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class DelusionReprise : EtherVeil {
    private ModifierKey Key { get; } = ModifierKey.EtherVeil(nameof(DelusionReprise));
    
    public override void Enable(Agent agent) {
        agent.Atk.AddUnique(new(Key, 50, ModifierType.CombatFlat));
    }
    public override void Disable(Agent agent) {
        agent.Atk.RemoveKey(Key);
    }

    public override string ToString() => nameof(DelusionReprise);
}