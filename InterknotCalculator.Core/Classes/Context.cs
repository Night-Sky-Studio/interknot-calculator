using System.Collections.ObjectModel;
using System.Collections.Specialized;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Classes.Events;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes;

public sealed class Context {
    public Dictionary<uint, Agent> Team { get; } = new();
    public uint MainAgentId { get; set; }
    public Agent MainAgent => Team[MainAgentId];

    public Enemy Enemy { get; init; } = new NotoriousDullahan();
    public List<AgentAction> Actions { get; } = [];
    public List<AgentAction> ActionsQueue { get; } = [];
    public EventBus Events { get; } = new();

    /// <remarks>
    /// Apparently, Jane's critting assault applies
    /// to the entire team, not just to her...
    /// </remarks>
    public double AnomalyCritMultiplier { get; set; } = 1;

    private List<EtherVeil> EtherVeils { get; set; } = [];
    public bool IsEtherVeilActive => EtherVeils.Count > 0;
    public T? GetEtherVeil<T>() where T : EtherVeil => EtherVeils.OfType<T>().FirstOrDefault();

    public void ActivateEtherVeil(Agent agent, EtherVeil veil) {
        veil.Activate(this);
        EtherVeils.Add(veil);
        Events.EtherVeilActivated(this, new(agent, veil));
    }
    public void DeactivateEtherVeil(Agent agent, EtherVeil veil) {
        veil.Deactivate(this);
        EtherVeils.Remove(veil);
        Events.EtherVeilDeactivated(this, new(agent, veil));
    }

    public void ProcessActionsQueue() { 
        if (ActionsQueue.Count > 0) {
            Actions.AddRange(ActionsQueue);
            ActionsQueue.Clear();
        }
    }
}