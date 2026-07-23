using InterknotCalculator.Core.Classes.EtherVeils;

namespace InterknotCalculator.Core.Interfaces;

public interface IEtherVeilAgent<out T> where T : EtherVeil {
    T EtherVeil { get; }
}