using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Core.Classes.EtherVeils;

public class Wellspring : EtherVeil {
    private ModifierKey Key { get; } = ModifierKey.EtherVeil<Wellspring>();

    protected override IEnumerable<Modifier> Initialize(Agent agent) => [
        agent.MaxHp.Add(Key, 0.05, ModifierType.CombatRatio)
    ];
    
    public override string ToString() => nameof(Wellspring);
}