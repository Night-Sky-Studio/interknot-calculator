using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class Wellspring : EtherVeil {
    private ModifierKey Key { get; } = ModifierKey.EtherVeil<DelusionReprise>();
    
    public override void Enable(Agent agent) {
        agent.MaxHp.AddUnique(new(Key, 0.05, ModifierType.CombatRatio));
    }
    public override void Disable(Agent agent) {
        agent.MaxHp.RemoveKey(Key);
    }
    
    public override string ToString() => nameof(Wellspring);
}