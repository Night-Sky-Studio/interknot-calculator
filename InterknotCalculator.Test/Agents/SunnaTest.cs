using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class SunnaTest : AgentsTest {
    [Test]
    public void CatsGazeAttackerTest() {
        var context = new Context();
        
        var ellen = new Ellen();
        ellen.RegisterHooks(context);
        var sunna = new Sunna();
        sunna.RegisterHooks(context);
        
        context.Team.Add(ellen.Id, ellen);
        context.Team.Add(sunna.Id, sunna);
        context.MainAgentId = ellen.Id;

        // context.Enemy.StunMultiplier = 1; // un-stun enemy
        
        context.Actions.AddRange(sunna.GetActionDamage(context, 
                new(SkillTag.ExSpecial, "special_photography_technique")));
        
        context.ProcessActionsQueue();
        
        context.Actions.AddRange(ellen.GetActionDamage(context, 
                new(SkillTag.BasicAtk, "saw_teeth_trimming")));
        context.ProcessActionsQueue();
        
        context.Actions.AddRange(ellen.GetActionDamage(context, 
                new(SkillTag.BasicAtk, "saw_teeth_trimming", 1)));
        context.ProcessActionsQueue();
        
        context.Actions.AddRange(ellen.GetActionDamage(context, 
                new(SkillTag.BasicAtk, "saw_teeth_trimming", 2)));
        context.ProcessActionsQueue();
        
        context.Actions.AddRange(ellen.GetActionDamage(context, 
            new(SkillTag.BasicAtk, "saw_teeth_trimming")));
        context.ProcessActionsQueue();
        
        Assert.That(context.Actions.Count, Is.AtLeast(4));
        Assert.That(context.Actions, Has.Some
            .Matches<AgentAction>(action => action.Name == "cat's_gaze"));
        
        PrintActions(context.Actions, context.Actions.Sum(a => a.AgentId == ellen.Id ? a.Damage : 0));
    }
    
}