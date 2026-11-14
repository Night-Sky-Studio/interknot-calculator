using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class ColdBlooded : EtherVeil {
    private ModifierKey Key { get; } = ModifierKey.EtherVeil<ColdBlooded>();

    protected override IEnumerable<Modifier> Initialize(Agent agent) => [
        agent.CritDamage.Add(Key, 0.05, ModifierType.CombatFlat)
    ];

    public override string ToString() => nameof(ColdBlooded);
}