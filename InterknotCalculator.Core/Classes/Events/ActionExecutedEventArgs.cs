using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes.Events;

public class ActionExecutedEventArgs(Agent agent, AgentAction action) : EventArgs {
    public Agent Agent { get; } = agent;
    public AgentAction Action { get; } = action;
}