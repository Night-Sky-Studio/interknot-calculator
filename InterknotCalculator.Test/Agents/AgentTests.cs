using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Server;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest : CalculatorTest {
    private DriveDisc[] GetDriveDiscs(CalcRequest req) => req.Discs.Select((d, idx) =>
        new DriveDisc(
            d.SetId, 
            Convert.ToUInt32(idx), 
            d.Rarity, 
            Stat.Stats[d.Stats[0]], 
            d.StatsLevels
                .Skip(1)
                .Select(p => Stat.SubStats[p.Key] with { Level = p.Value })
                .ToArray())
    ).ToArray();

    private void PrintActions(IEnumerable<AgentAction> actions) {
        foreach (var action in actions) {
            Console.WriteLine($"{action.AgentId, -6}{action.Name, -38}{action.Tag, -24}{action.Damage}");
        }
    }
}