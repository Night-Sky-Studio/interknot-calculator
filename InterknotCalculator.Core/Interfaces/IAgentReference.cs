using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Interfaces;

public interface IAgentReference<out TAgent> where TAgent : Agent {
    /// <summary>
    /// Provides Agent's Reference Build
    /// </summary>
    /// <param name="weaponId">Weapon's passive to activate</param>
    /// <param name="setId">Drive Disc Set's passive to activate</param>
    /// <remarks>
    /// Reference build is a build that achieves the maximum
    /// agent's external bonus value possible (if capped, reasonable otherwise).
    /// </remarks>
    /// <returns>Instance of Agent's Reference build</returns>
    public static abstract TAgent Reference(uint weaponId, uint setId);
}