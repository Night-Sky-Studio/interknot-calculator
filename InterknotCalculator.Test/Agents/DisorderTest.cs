using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class DisorderTests : AgentsTest {
    protected override CalcRequest Request => JaneTests.Jane;

    [Test]
    public async Task DisorderTest() {
        var prevTeam = Request.Team.Clone();
        var prevRotation = Request.Rotation.Clone();
        Request.Team = [
            new(AgentId.Burnice),
            new(AgentId.Lucy)
        ];
        Request.Rotation = [
            "1171.energizing_speciality_drink",
            "1171.intense_heat_stirring_method",
            "1171.intense_heat_stirring_method_double",
            "1171.fuel-fed_flame",
            "flowers_of_sin",
            "phantom_thrust",
            "salchow_jump 1",
            "salchow_jump 2",
            "aerial_sweep_cleanout",
            "aerial_sweep_cleanout",
            "phantom_thrust",
            "final_curtain",
        ];
        var result = Calculator.Calculate(Request);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        
        Assert.That(result.PerAction, Has.Exactly(6)
            .Matches<AgentAction>(action => action.Tag is SkillTag.AttributeAnomaly));

        Assert.That(result.PerAction, Has.Exactly(1).Matches<AgentAction>(action => action is {
            Name: "disorder", AgentId: AgentId.Jane, Damage: > 0
        }));
        
        foreach (var action in Request.Rotation) {
            var act = RotationAction.Parse(action, AgentId.Jane);
            if (act is null) {
                Assert.Fail($"Failed to parse action: {action}");
                return;
            }

            Assert.That(result.PerAction, Has.Some.Matches<AgentAction>(a =>
                a.Name.Contains(act.ActionName)));
        }
        
        await VerifyActions(result.PerAction);
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction, result.Total);
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', result.Enemy.AnomalyBuildup)}");
        
        Request.Team = (TeamMemberRequest[])prevTeam;
        Request.Rotation = (string[])prevRotation;
    }
}