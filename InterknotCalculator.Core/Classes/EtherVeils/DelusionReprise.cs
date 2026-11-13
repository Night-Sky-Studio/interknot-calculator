using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class DelusionReprise : EtherVeil {
    private ModifierKey Key { get; } = ModifierKey.EtherVeil<DelusionReprise>();

    protected override IEnumerable<Modifier> Initialize(Agent agent) => [
        agent.Atk.Add(Key, 50, ModifierType.CombatFlat)
    ];

    public override string ToString() => nameof(DelusionReprise);
}