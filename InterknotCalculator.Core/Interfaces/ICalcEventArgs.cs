using InterknotCalculator.Core.Classes.Agents;

namespace InterknotCalculator.Core.Interfaces;

public interface ICalcEventArgs {
    Agent Agent { get; }
}