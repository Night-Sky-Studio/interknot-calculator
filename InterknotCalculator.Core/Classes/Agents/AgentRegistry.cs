using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes.Agents;

public static partial class AgentRegistry {
    private static readonly Dictionary<uint, Func<uint, Agent>> Instances = new();
    private static readonly Dictionary<uint, Func<uint, uint, Agent>> References = new();

    public static Agent CreateInstance(uint agentId, uint mindscape = 0) {
        if (!Instances.TryGetValue(agentId, out var f))
            throw new ArgumentOutOfRangeException(nameof(agentId), agentId,
                $"Agent {agentId} is not implemented.");
        return f(mindscape);
    }

    public static Agent CreateReference(TeamMemberRequest request) {
        if (!References.TryGetValue(request.AgentId, out var f))
            throw new ArgumentOutOfRangeException(nameof(request.AgentId), request.AgentId,
                $"Reference build for agent {request.AgentId} is not implemented.");
        return f(request.WeaponId, request.DriveDiscSetId);
    }
}