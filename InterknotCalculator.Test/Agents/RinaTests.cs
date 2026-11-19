using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class RinaTests {
    [Test]
    public void RinaPotentialBuffsTest() {
        var rina = Rina.Reference(WeaponId.WeepingCradle, DriveDiscSetId.MoonlightLullaby);
        var ellen = new Ellen();

        var ctx = new Context {
            Team = {
                [AgentId.Ellen] = ellen, [AgentId.Rina] = rina
            },
            MainAgentId = AgentId.Ellen
        };

        foreach (var agent in ctx.Team.Values) {
            agent.RegisterHooks(ctx);
            agent.Weapon?.RegisterHooks(ctx, agent.Id);
            foreach (var set in agent.FullSets)
                set.RegisterHooks(ctx, agent.Id);
        }

        ctx.Events.CalculationStarted(ctx);

        Assert.That(rina.PenRatio.Value, Is.EqualTo(0.72).Within(1e-9));
        
        var atkMod = ellen.Atk.Modifiers.FirstOrDefault(m => 
            m.Key == ModifierKey.Agent(AgentId.Rina) + ModifierKey.Potential(6));
        Assert.That(atkMod, Is.Not.Null);
        Assert.That(atkMod.Value, Is.EqualTo(576).Within(1e-9));
        
        var defMod = ellen.Def.Modifiers.FirstOrDefault(m => 
            m.Key == ModifierKey.Agent(AgentId.Rina) + ModifierKey.Potential(6));
        Assert.That(defMod, Is.Not.Null);
        Assert.That(defMod.Value, Is.EqualTo(468).Within(1e-9));
        
        var penRatioMod = ellen.PenRatio.Modifiers.FirstOrDefault(m => 
            m.Key == ModifierKey.Agent(AgentId.Rina) + ModifierKey.CorePassive());
        Assert.That(penRatioMod, Is.Not.Null);
        Assert.That(penRatioMod.Value, Is.EqualTo(0.3).Within(1e-9));
    }
}