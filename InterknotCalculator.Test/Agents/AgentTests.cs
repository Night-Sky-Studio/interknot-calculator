using System.Globalization;
using System.Reflection;
using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

public abstract class AgentsTest {
    private static readonly Dictionary<uint, string> AgentIdNames =
        typeof(AgentId)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && f.FieldType == typeof(uint))
            .ToDictionary(f => (uint)f.GetRawConstantValue()!, f => f.Name);

    private static string GetAgentName(uint id) =>
        AgentIdNames.TryGetValue(id, out var name) ? name : id.ToString();

    protected Task VerifyActions(IEnumerable<AgentAction> actions) =>
        Verify(actions.Select(a => new {
            a.Name,
            a.Tag,
            a.Damage
        }));
    
    protected void PrintActions(IEnumerable<AgentAction> actions, double total) {
        foreach (var action in actions) {
            Console.WriteLine(
                $"{GetAgentName(action.AgentId),-16}{action.Name,-48}{action.Tag,-24}{action.Damage.ToString(CultureInfo.InvariantCulture)}");
        }

        Console.WriteLine($"Total: {total.ToString(CultureInfo.InvariantCulture)}");
    }
    
    /// <summary>
    /// Verifies that every requested rotation action appears in <paramref name="actions"/>
    /// in the SAME relative order as requested, allowing the Calculator to insert
    /// additional actions (anomalies, disorders, aftershocks, macro expansions) in-between.
    /// I.e., the requested rotation must be an ordered subsequence of the produced actions.
    /// </summary>
    protected static void AssertRotationOrderPreserved(
        IEnumerable<string> rotation,
        IReadOnlyList<AgentAction> actions,
        uint fallbackAgentId) {
        // Parse the requested rotation into (agentId, actionName) pairs, in order.
        var expected = rotation
            .Select(r => RotationAction.Parse(r, fallbackAgentId))
            .Select((a, i) => (parsed: a, index: i))
            .ToList();

        // Fail fast on unparsable entries so the message is clear.
        var bad = expected.FirstOrDefault(e => e.parsed is null);
        if (bad.parsed is null && expected.Any(e => e.parsed is null)) {
            Assert.Fail($"Failed to parse rotation action at index {bad.index}");
        }

        var expectedActions = expected.Select(e => e.parsed!).ToList();

        // Walk the produced actions once, advancing through the expected rotation.
        var cursor = 0;
        foreach (var action in actions) {
            if (cursor >= expectedActions.Count) break;

            var next = expectedActions[cursor];
            if (action.AgentId == next.AgentId && action.Name.Contains(next.ActionName)) {
                cursor++;
            }
        }

        if (cursor >= expectedActions.Count) return;
        
        var missing = expectedActions[cursor];
        Assert.Fail(
            $"Rotation order not preserved. Could not match requested action " +
            $"'{missing.ActionName}' (agent {missing.AgentId}) at position {cursor} " +
            $"as an in-order subsequence of the produced actions.\n" +
            $"Produced order:\n  {string.Join("\n  ", actions.Select(a => $"{a.AgentId}:{a.Name} [{a.Tag}]"))}");
    }
    
    protected abstract CalcRequest Request { get; }
}