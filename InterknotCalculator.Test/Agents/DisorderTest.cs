using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest : CalculatorTest {
    [Test]
    public void DisorderTest() {
        var prevTeam = Jane.Team.Clone();
        var prevRotation = Jane.Rotation.Clone();
        Jane.Team = [
            1171, // Burnice
            1151  // Lucy
        ];
        Jane.Rotation = [
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
        var enemy = new NotoriousDullahan();
        
        var result = Calculator.Calculate(Jane.AgentId, Jane.WeaponId, GetDriveDiscs(Jane), 
            Jane.Team, Jane.Rotation, enemy);
        
        Assert.That(result.PerAction, Is.Not.Empty);
        // Assert.That(result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly), Is.EqualTo(3));
        
        Console.WriteLine($"Total Anomaly triggers: {result.PerAction.Count(action => action.Tag == SkillTag.AttributeAnomaly)}");
        PrintActions(result.PerAction);
        Console.WriteLine($"Total: {result.Total}");
        Console.WriteLine($"\nEnemy anomaly\n{string.Join('\n', enemy.AnomalyBuildup)}");
        
        Jane.Team = (uint[])prevTeam;
        Jane.Rotation = (string[])prevRotation;
    }
}