using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Events;

public class ActionExecutedEventArgs(Agent agent, Ability ability) : EventArgs, ICalcEventArgs {
    
    
    public Agent Agent { get; } = agent;
    public Ability Ability { get; } = ability;
}