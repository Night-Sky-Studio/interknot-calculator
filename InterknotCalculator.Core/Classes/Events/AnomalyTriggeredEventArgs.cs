using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

public class AnomalyTriggeredEventArgs(Agent agent, Element element) : EventArgs, ICalcEventArgs {
    public Agent Agent { get; } = agent;
    public Element Element { get; } = element;
}