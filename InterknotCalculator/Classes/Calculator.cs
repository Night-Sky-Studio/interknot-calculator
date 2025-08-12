using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes;

/// <summary>
/// Damage Calculator
/// </summary>
public class Calculator {
    /// <summary>
    /// Creates an Agent instance from known agents IDs
    /// </summary>
    /// <param name="agentId">Agent ID</param>
    /// <returns><see cref="Agent"/> instance with specific implementation</returns>
    /// <exception cref="ArgumentOutOfRangeException">Agent not implemented</exception>
    private static Agent CreateAgentInstance(uint agentId) {
        return agentId switch {
            1041 => new Soldier11(),
            1091 => new Miyabi(),
            1171 => new Burnice(),
            1181 => new Grace(),
            1191 => new Ellen(),
            1201 => new Harumasa(),
            1221 => new Yanagi(),
            1241 => new ZhuYuan(),
            1261 => new JaneDoe(),
            1321 => new Evelyn(),
            1331 => new Vivian(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent instance wasn't found.")
        };
    }

    private static Agent CreateAgentReference(uint agentId) {
        return agentId switch {
            1031 => Nicole.Reference(), // Support agent - provide reference implementation
            1131 => Soukaku.Reference(),
            1141 => Lycaon.Reference(),
            1151 => Lucy.Reference(),
            1171 => Burnice.Reference(),
            1211 => Rina.Reference(),
            1261 => JaneDoe.Reference(),
            1311 => AstraYao.Reference(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent reference wasn't found.")
        };
    }

    /// <summary>
    /// Main damage calculation function
    /// </summary>
    /// <param name="characterId">Agent ID</param>
    /// <param name="weaponId">Weapon ID</param>
    /// <param name="driveDiscs">Agent's equipped Drive Discs</param>
    /// <param name="team">Team members IDs collection (except current agent)</param>
    /// <param name="rotation">Collection of agent skills</param>
    /// <param name="enemy">Enemy instance</param>
    /// <returns>A collection of agent actions</returns>
    public CalcResult Calculate(uint characterId, uint weaponId, 
        List<DriveDisc> driveDiscs, IEnumerable<uint> team, 
        IEnumerable<string> rotation, Enemy enemy) {
        
        // Initialize the agent and the weapon
        // Having a dictionary here allows us to use abilities 
        // of other team members
        var fullTeam = new Dictionary<uint, Agent> {
            [characterId] = CreateAgentInstance(characterId)
        };
        
        fullTeam[characterId].SetWeapon(weaponId);
        fullTeam[characterId].SetDriveDiscs(driveDiscs);
        
        var actions = new List<AgentAction>();
        
        // Apply team passive
        // Those include current agent in the team, because current agent can also
        // have synergy with other team members
        foreach (var member in team) {
            fullTeam[member] = CreateAgentReference(member);
        }
        List<Stat> fullTeamPassive = [];

        List<AgentAction> anomalyQueue = [];

        enemy.AttributeAnomalyTrigger = (sender, element, agentId) => {
            var isFrostburnShatter = element == Element.Ice && sender.AfflictedAnomaly?.Element == Element.Frost;

            // Process anomaly damage
            anomalyQueue.Add(fullTeam[agentId].GetAnomalyDamage(element, enemy));

            // Then process disorders
            if (sender.AfflictedAnomaly is { } anomaly && !isFrostburnShatter) {
                if (anomaly.Element != element) {
                    anomalyQueue.Add(fullTeam[agentId].GetAnomalyDamage(Element.None, enemy));
                } else {
                    sender.AfflictedAnomaly = null;
                }
            }
        };
        
        // All supports are rocking "Astral Voice" set
        // they need to be counted to then subtract excess applications of this set
        // as "Astral Voice" 4pc bonus can only be applied once
        var astralVoiceCount = fullTeam.Values.Count(a => a.Speciality == Speciality.Support);
        foreach (var a in fullTeam.Values) {
            fullTeamPassive.AddRange(a.ApplyTeamPassive([..fullTeam.Values]));
        }
        
        foreach (var stat in fullTeamPassive) {
            if (stat.SkillTags.Length > 0) {
                fullTeam[characterId].TagBonus.Add(stat);
            } else {
                fullTeam[characterId].BonusStats[stat.Affix] += stat.Value;
            }
        }

        foreach (var a in fullTeam.Values) {
            foreach (var (afx, bonus) in a.ExternalBonus) {
                fullTeam[characterId].BonusStats[afx] += bonus;
            }

            foreach (var stat in a.ExternalTagBonus) {
                fullTeam[characterId].TagBonus.Add(stat);
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
            fullTeam[characterId].BonusStats[avBonus.Affix] -= avBonus.Value * (astralVoiceCount - 1);
        }

        if (fullTeam.TryGetValue(1331, out var agent) && agent is Vivian vivian) {
            // Apply Vivian's Action Handler to all team members
            foreach (var a in fullTeam.Values) {
                a.OnAction = (sender, tag, e) => {
                    if (tag is not (SkillTag.ExSpecial or SkillTag.AttributeAnomaly) || e.AfflictedAnomaly is null) return;
                    if (vivian.GuardFeathersCount == 0) return;
                    var anomalyElement = e.AfflictedAnomaly.Element;
                    if (sender.Anomalies.TryGetValue(anomalyElement, out var previousAnomaly)) {
                        var abloomAnomaly = vivian.CreateAbloom(anomalyElement);
                        sender.Anomalies[anomalyElement] = previousAnomaly with {
                            Scale = abloomAnomaly.Scale
                        };
                    } else {
                        sender.Anomalies[anomalyElement] = vivian.CreateAbloom(anomalyElement);
                    }

                    var abloom = sender.GetAnomalyDamage(anomalyElement, e, true);
                    anomalyQueue.Add(abloom with {
                        AgentId = vivian.Id,
                        Name = $"abloom_{e.AfflictedAnomaly}",
                    });

                    if (previousAnomaly is not null) {
                        sender.Anomalies[anomalyElement] = previousAnomaly;
                    } else {
                        sender.Anomalies.Remove(anomalyElement);
                    }
                };
            }
        }
        
        // Do the calculation
        // Anomalies should be included automatically
        foreach (var action in rotation) {
            var act = RotationAction.Parse(action, characterId);
            if (act is null) {
                throw new ArgumentException($"Invalid action: {action}");
            }
            
            actions.AddRange(fullTeam[act.AgentId].GetActionDamage(act.ActionName, act.Scale - 1, enemy));
            
            // Anomalies are processed in a queue to maintain the order
            // This includes simultaneous anomaly triggers like disorders
            if (anomalyQueue.Count > 0) {
                actions.AddRange(anomalyQueue);
                anomalyQueue.Clear();
            }
        }
        
        // Cleanup actions - remove all damage/daze from other agents
        actions = actions.Select(action => action.AgentId != characterId
            ? action with { Damage = 0, Daze = 0 } 
            : action
        ).ToList();

        return new CalcResult {
            FinalStats = {
                BaseStats = fullTeam[characterId].BaseStats,
                CalculatedStats = fullTeam[characterId].CollectStats()
            },
            Enemy = enemy,
            PerAction = actions,
            Total = actions.Sum(action => action.Damage)
        };
    }
}