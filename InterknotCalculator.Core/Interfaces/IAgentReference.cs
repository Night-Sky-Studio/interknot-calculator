using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Interfaces;

public interface IAgentReference<out T> where T : Agent {
    /// <summary>
    /// Provides Agent's Reference Build
    /// </summary>
    /// <remarks>
    /// Reference build is a build that achieves the maximum
    /// agent's external bonus value possible (if capped, reasonable otherwise).
    /// </remarks>
    /// <returns>Instance of Agent's Reference build</returns>
    public static abstract T Reference();
}