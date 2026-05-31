using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes;

/// <summary>
/// Damage Calculator
/// </summary>
public class Calculator {
    /// <summary>
    /// Creates an Agent instance from known agents IDs
    /// </summary>
    /// <param name="agentId">Agent ID</param>
    /// <param name="mindscape">Mindscape Level</param>
    /// <returns><see cref="Agent"/> instance with specific implementation</returns>
    /// <exception cref="ArgumentOutOfRangeException">Agent not implemented</exception>
    private static Agent CreateAgentInstance(uint agentId, uint mindscape = 0) {
        return agentId switch {
            AgentId.Soldier11    => new Soldier11(),
            AgentId.Miyabi       => mindscape switch {
                0 => new Miyabi(),
                1 => new MiyabiM1(),
                2 => new MiyabiM2(),
                _ => throw new ArgumentOutOfRangeException(nameof(mindscape), mindscape, "Invalid mindscape value.")
            },
            AgentId.Burnice      => new Burnice(),
            AgentId.Grace        => new Grace(),
            AgentId.Ellen        => mindscape switch {
                0 => new Ellen(),
                1 => new EllenM1(),
                2 => new EllenM2(),
                3 => new EllenM3(),
                4 => new EllenM4(),
                5 => new EllenM5(),
                6 => new EllenM6(),
                 _ => throw new ArgumentOutOfRangeException(nameof(mindscape), mindscape, "Invalid mindscape value.")
            },
            AgentId.Harumasa     => new Harumasa(),
            AgentId.Yanagi       => new Yanagi(),
            AgentId.ZhuYuan      => new ZhuYuan(),
            AgentId.Jane         => mindscape switch {
                0 => new JaneDoe(),
                1 => new JaneDoeM1(),
                2 => new JaneDoeM2(),
                 _ => throw new ArgumentOutOfRangeException(nameof(mindscape), mindscape, "Invalid mindscape value.")
            },
            AgentId.Evelyn       => new Evelyn(),
            AgentId.Vivian       => new Vivian(),
            AgentId.Soldier0Anby => new SilverAnby(),
            AgentId.Trigger      => new Trigger(),
            AgentId.Yixuan       => new Yixuan(),
            AgentId.Alice        => new Alice(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent instance wasn't found.")
        };
    }

    private static Agent CreateAgentReference(uint agentId) {
        return agentId switch {
            AgentId.Nicole       => Nicole.Reference(), // Support agent - provide reference implementation
            AgentId.Koleda       => Koleda.Reference(),
            AgentId.Soukaku      => Soukaku.Reference(),
            AgentId.Lycaon       => Lycaon.Reference(),
            AgentId.Lucy         => Lucy.Reference(),
            AgentId.Burnice      => Burnice.Reference(),
            AgentId.Rina         => Rina.Reference(),
            AgentId.Jane         => JaneDoe.Reference(),
            AgentId.AstraYao     => AstraYao.Reference(),
            AgentId.Soldier0Anby => SilverAnby.Reference(),
            AgentId.Trigger      => Trigger.Reference(),
            AgentId.PanYinhu     => PanYinhu.Reference(),
            AgentId.JuFufu       => JuFufu.Reference(),
            AgentId.Vivian       => Vivian.Reference(),
            AgentId.Yuzuha       => Yuzuha.Reference(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent reference wasn't found.")
        };
    }

    /// <summary>
    /// Main damage calculation function
    /// </summary>
    /// <param name="characterId">Agent ID</param>
    /// <param name="weaponId">Weapon ID</param>
    /// <param name="mindscape">Mindscape Level</param>
    /// <param name="driveDiscs">Agent's equipped Drive Discs</param>
    /// <param name="team">Team members IDs collection (except current agent)</param>
    /// <param name="rotation">Collection of agent skills</param>
    /// <param name="enemy">Enemy instance</param>
    /// <param name="calcType">Damage or Daze</param>
    /// <returns>A collection of agent ctx.Actions</returns>
    public CalcResult Calculate(uint characterId, uint weaponId,
        DriveDisc[] driveDiscs, IEnumerable<uint> team, 
        IEnumerable<string> rotation, Enemy enemy, CalculationType calcType = CalculationType.Damage,
        uint mindscape = 0) {

        var ctx = new Context {
            Team = {
                // Initialize the agent and the weapon
                // Having a dictionary here allows us to use abilities 
                // of other team members
                [characterId] = CreateAgentInstance(characterId, mindscape)
            },
            MainAgentId = characterId,
            Enemy = enemy
        };

        ctx.MainAgent.SetWeapon(weaponId);
        ctx.MainAgent.SetDriveDiscs(driveDiscs);
        ctx.MainAgent.Hp = ctx.Team[characterId].MaxHp;
        
        // Apply team passive
        // Those include current agent in the team, because current agent can also
        // have synergy with other team members
        foreach (var member in team) {
            ctx.Team[member] = CreateAgentReference(member);
        }
        List<Stat> fullTeamPassive = [];

        ctx.Events.OnAnomalyTriggered.Add((c, e) => {
            var element = e.Element;
            var sender = c.Enemy;
            var agentId = e.Agent.Id;
            
            var isFrostburnShatter = element == Element.Ice && sender.AfflictedAnomaly?.Element == Element.Frost;

            // Process anomaly damage
            var action = ctx.Team[agentId].GetAnomalyDamage(ctx, element, true);
            ctx.ActionsQueue.Add(action);
            ctx.Events.ActionExecuted(ctx, new(e.Agent, new(SkillTag.AttributeAnomaly, action.Name)));

            // Then process disorders
            if (sender.AfflictedAnomaly is { } anomaly && !isFrostburnShatter) {
                if (anomaly.Element != element || anomaly.SelfDisorder) {
                    ctx.ActionsQueue.Add(ctx.Team[agentId].GetAnomalyDamage(ctx, Element.None));
                } 
                // else {
                //     sender.AfflictedAnomaly = null;
                // }
            }
        });
        
        // All supports are rocking "Astral Voice" set
        // they need to be counted to then subtract excess applications of this set
        // as "Astral Voice" 4pc bonus can only be applied once
        var astralVoiceCount = ctx.Team.Values.Count(a => a.Speciality == Speciality.Support);
        foreach (var a in ctx.Team.Values) {
            fullTeamPassive.AddRange(a.ApplyTeamPassive([..ctx.Team.Values]));
            a.RegisterHooks(ctx);
        }
        
        foreach (var stat in fullTeamPassive) {
            foreach (var character in ctx.Team.Values) {
                if (stat.SkillTags.Length > 0) {
                    character.TagBonus.Add(stat);
                } else {
                    character.BonusStats[stat.Affix] += stat.Value;
                }
            }
        }

        foreach (var a in ctx.Team.Values) {
            foreach (var (afx, bonus) in a.ExternalBonus) {
                ctx.Team[characterId].BonusStats[afx] += bonus;
            }

            foreach (var stat in a.ExternalTagBonus) {
                ctx.Team[characterId].TagBonus.Add(stat);
            }

            if (a is IStunAgent stunAgent) {
                enemy.StunMultiplier += stunAgent.EnemyStunBonusOverride;
            }
        }
        
        // Subtract excess applications of "Astral Voice" 4pc bonus
        // I can't believe how hacky this feels...
        if (astralVoiceCount > 1) {
            var astralVoice = Resources.Current.GetDriveDiscSet(32800);
            var avBonus = astralVoice.FullBonus.First();
            ctx.Team[characterId].BonusStats[avBonus.Affix] -= avBonus.Value * (astralVoiceCount - 1);
        }
        
        // Do the calculation
        // Anomalies should be included automatically
        foreach (var action in rotation) {
            var act = RotationAction.Parse(action, characterId);
            if (act is null) {
                throw new ArgumentException($"Invalid action: {action}");
            }
            
            var agent = ctx.Team[act.AgentId];
            var ability = act.ToAbility(agent);
            // Step 1: Perform action
            ctx.Actions.AddRange(agent.GetActionDamage(ctx, ability));
            
            // Step 2: Process side effects
            // Anomalies are processed in a queue to maintain the order
            // This includes simultaneous anomaly triggers like disorders
            ctx.ProcessActionsQueue();
            
            // Step 3: Perform Aftershocks
            // These happen after main anomaly triggers
            ctx.Events.Aftershock(ctx, new(agent, ability));
            
            // Step 4: Process Aftershock side effects
            ctx.ProcessActionsQueue();
        }
        
        // Cleanup ctx.Actions - remove all damage/daze from other agents
        ctx.Actions.ForEach(action => {
            if (action.AgentId == characterId) return;
            action.Damage = 0;
            action.Daze = 0;
        });

        var total = calcType switch {
            CalculationType.Damage => ctx.Actions.Sum(action => action.Damage),
            CalculationType.Daze   => ctx.Actions.Sum(action => action.Daze),
            _                      => ctx.Actions.Sum(action => action.Damage) // Fallback to damage
        };
        
        return new CalcResult {
            FinalStats = {
                BaseStats = ctx.MainAgent.BaseStats,
                CalculatedStats = ctx.MainAgent.CollectStats()
            },
            Enemy = enemy,
            PerAction = ctx.Actions,
            Total = total
        };
    }
}