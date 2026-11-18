using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

/// <param name="agent">Agent that triggered Maim</param>
public class MaimEventArgs(Agent agent) : EventArgs, ICalcEventArgs {
    public Agent Agent { get; } = agent;
}
