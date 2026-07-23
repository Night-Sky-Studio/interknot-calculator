using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

public class EtherVeilEventArgs(Agent agent, EtherVeil etherVeil) : EventArgs, ICalcEventArgs {
    public Agent Agent { get; } = agent;
    public EtherVeil EtherVeil { get; } = etherVeil;
}