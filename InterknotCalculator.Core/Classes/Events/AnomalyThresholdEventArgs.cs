using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

public class AnomalyThresholdEventArgs(Agent agent, Element element, bool ignore = false) : EventArgs, ICalcEventArgs {
    public Agent Agent { get; } = agent;
    public Element Element { get; } = element;
    public bool Ignore { get; set; } = ignore;
}