using InterknotCalculator.Classes.Agents;

namespace InterknotCalculator.Classes;

public record RotationAction(uint AgentId, string ActionName, int Scale) {
    public static RotationAction? Parse(string rotation, uint fallbackAgentId = 0) {
        // [AgentId.]ActionName [Scale]
        var actionScale = rotation.Split(' ');
        if (actionScale.Length < 1) return null;
        
        var action = actionScale[0].Split('.');

        var actionName = action.Length == 1 ? action[0] : action[1];

        if (string.IsNullOrWhiteSpace(actionName)) {
            return null;
        }
        
        if (string.IsNullOrWhiteSpace(action[0])) {
            return null;
        }
        
        if (!uint.TryParse(action[0], out var agentId)) {
            agentId = fallbackAgentId;
        }
        if (agentId == 0) {
            return null; // Invalid agent ID
        }

        var scale = 1;
        if (actionScale.Length > 1 && !int.TryParse(actionScale[1], out scale)) {
            return null;
        }

        return new(agentId, actionName, scale);
    }
}