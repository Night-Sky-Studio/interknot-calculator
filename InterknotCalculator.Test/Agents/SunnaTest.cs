using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test.Agents;

[TestFixture]
public class SunnaTest : AgentsTest {
    [Test]
    public void CatsGaze_AttackerTest() {
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
    
    [Test]
    public void CatsGazeTwoActivationCyclesWithNoClawSharpenersTest() {
        var context = new Context();

        var ellen = new Ellen();
        ellen.RegisterHooks(context);
        var sunna = new Sunna();
        sunna.RegisterHooks(context);

        context.Team.Add(ellen.Id, ellen);
        context.Team.Add(sunna.Id, sunna);
        context.MainAgentId = ellen.Id;

        // Using "naughty_cat_spotted" instead of "special_photography_technique"
        // to activate Cat's Gaze here, since the latter also reactivates
        // the Ether Veil, which grants Claw Sharpeners and would defeat
        // the "no remaining Claw Sharpeners" premise of this test

        // Cycle 1: Activate Cat's Gaze
        context.Actions.AddRange(sunna.GetActionDamage(context,
            new(SkillTag.BasicAtk, "naughty_cat_spotted")));
        context.ProcessActionsQueue();

        // Let it fire and fully expire, since no Claw Sharpeners
        // are available to reapply it (the default enemy is stunned,
        // so the cooldown is fully consumed on this single action)
        context.Actions.AddRange(ellen.GetActionDamage(context,
            new(SkillTag.BasicAtk, "saw_teeth_trimming")));
        context.ProcessActionsQueue();
        
        // Cycle 2: Reactivate Cat's Gaze after it has already expired once
        context.Actions.AddRange(sunna.GetActionDamage(context,
            new(SkillTag.BasicAtk, "naughty_cat_spotted")));
        context.ProcessActionsQueue();

        context.Actions.AddRange(ellen.GetActionDamage(context,
            new(SkillTag.BasicAtk, "saw_teeth_trimming")));
        context.ProcessActionsQueue();

        var catsGazeActivations = context.Actions
            .Count(action => action.Name == "cat's_gaze");

        // Cat's Gaze must trigger once per cycle, twice in total
        Assert.That(catsGazeActivations, Is.EqualTo(2));

        PrintActions(context.Actions, context.Actions.Sum(a => a.AgentId == ellen.Id ? a.Damage : 0));
    }
}