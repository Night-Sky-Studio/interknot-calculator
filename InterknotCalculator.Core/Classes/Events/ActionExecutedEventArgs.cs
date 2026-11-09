using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes.Events;

public class ActionExecutedEventArgs(Agent agent, Ability ability, AgentAction action) : EventArgs {
    public Agent Agent { get; } = agent;
    public Ability Ability { get; } = ability;
    public AgentAction Action { get; } = action;
}