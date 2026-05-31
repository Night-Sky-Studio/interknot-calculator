using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

public class AnomalyBuildupEventArgs(Agent agent, double buildup, Element element) : EventArgs, ICalcEventArgs {
    public Agent Agent { get; } = agent;
    public double Buildup { get; } = buildup;
    public Element Element { get; } = element;
}