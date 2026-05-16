using System.Globalization;
using System.Reflection;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

public abstract class AgentsTest : CalculatorTest {
    private static readonly Dictionary<uint, string> AgentIdNames =
        typeof(AgentId)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && f.FieldType == typeof(uint))
            .ToDictionary(f => (uint)f.GetRawConstantValue()!, f => f.Name);
    
    protected static string GetAgentName(uint id) =>
        AgentIdNames.TryGetValue(id, out var name) ? name : id.ToString();
    
    protected DriveDisc[] GetDriveDiscs(CalcRequest req) => req.Discs.Select((d, idx) =>
        new DriveDisc(d.SetId, Convert.ToUInt32(idx), d.Rarity, Stat.Stats[d.Stats[0]],
            d.StatsLevels.Skip(1).Select(p => Stat.SubStats[p.Key] with {
                Level = p.Value
            }))).ToArray();

    protected void PrintActions(IEnumerable<AgentAction> actions, double total) {
        foreach (var action in actions) {
            Console.WriteLine($"{GetAgentName(action.AgentId), -12}{action.Name, -38}{action.Tag, -24}{action.Damage.ToString(CultureInfo.InvariantCulture)}");
        }
        Console.WriteLine($"Total: {total.ToString(CultureInfo.InvariantCulture)}");
    }
}