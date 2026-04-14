using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

public class ActionExecutedEventArgs(Agent agent, SkillTag tag) : EventArgs, ICalcEventArgs {
    public Agent Agent { get; } = agent;
    public SkillTag Tag { get; } = tag;
}