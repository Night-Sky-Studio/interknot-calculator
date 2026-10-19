namespace InterknotCalculator.Core.Classes.Server;

public record ActionGroup(
    uint AgentId,
    string Name,
    IEnumerable<Guid> ActionIds
);