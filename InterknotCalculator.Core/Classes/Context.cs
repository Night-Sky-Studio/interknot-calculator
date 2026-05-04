using System.Collections.Immutable;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Events;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes;

public sealed class Context {
    public Dictionary<uint, Agent> Team { get; } = new();
    public uint MainAgentId { get; set; }
    public Agent MainAgent => Team[MainAgentId];
    
    public Enemy Enemy { get; } = new NotoriousDullahan();
    public List<AgentAction> Actions { get; } = [];
    public List<AgentAction> ActionsQueue { get; } = [];
    public EventBus Events { get; } = new();

    /// <remarks>
    /// Apparently, Jane's critting assault applies
    /// to the entire team, not just to her...
    /// </remarks>
    public double AnomalyCritMultiplier { get; set; } = 1;
}