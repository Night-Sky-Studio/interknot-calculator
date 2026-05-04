using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Interfaces;

public interface IPolarityDisorderAgent {
    AgentAction GetPolarityDisorder(Context ctx);
}