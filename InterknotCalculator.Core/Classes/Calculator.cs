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
    /// <returns><see cref="Agent"/> instance with specific implementation</returns>
    /// <exception cref="ArgumentOutOfRangeException">Agent not implemented</exception>
    private static Agent CreateAgentInstance(uint agentId) {
        return agentId switch {
            AgentId.Soldier11    => new Soldier11(),
            AgentId.Miyabi       => new Miyabi(),
            AgentId.Burnice      => new Burnice(),
            AgentId.Grace        => new Grace(),
            AgentId.Ellen        => new Ellen(),
            AgentId.Harumasa     => new Harumasa(),
            AgentId.Yanagi       => new Yanagi(),
            AgentId.ZhuYuan      => new ZhuYuan(),
            AgentId.Jane         => new JaneDoe(),
            AgentId.Evelyn       => new Evelyn(),
            AgentId.Vivian       => new Vivian(),
            AgentId.Soldier0Anby => new SilverAnby(),
            AgentId.Trigger      => new Trigger(),
            AgentId.Yixuan       => new Yixuan(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent instance wasn't found.")
        };
    }

    private static Agent CreateAgentReference(uint agentId) {
        return agentId switch {
            AgentId.Nicole       => Nicole.Reference(), // Support agent - provide reference implementation
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
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent reference wasn't found.")
        };
    }
    /*
    /// <summary>
    /// Collects all unconditional stats from Drive Discs
    /// </summary>
    /// <param name="driveDiscs">A collection of <see cref="DriveDisc"/> instances</param>
    /// <param name="partialSets">
    /// A collection of Drive Disc Set IDs for
    /// which 2pc bonus should be applied
    /// </param>
    /// <param name="tagDamageBonus">Tag Damage Bonus dictionary reference</param>
    /// <returns>A dictionary of all collected stats</returns>
    private SafeDictionary<Affix, double> CollectDriveDiscStats(IEnumerable<DriveDisc> driveDiscs, 
        IEnumerable<uint> partialSets, List<Stat> tagDamageBonus) {
        var result = new SafeDictionary<Affix, double>();

        foreach (var disc in driveDiscs) {
            result[disc.MainStat.Affix] += disc.MainStat.Value;
            foreach (var subStat in disc.SubStats) {
                result[subStat.Stat.Affix] += subStat.Level * subStat.Stat.Value;
            }
        }

        foreach (var set in partialSets) {
            var dds = Resources.Current.GetDriveDiscSet(set);
            foreach (var bonus in dds.PartialBonus) {
                if (bonus.SkillTags.Length != 0) {
                    tagDamageBonus.Add(bonus);
                } else
                    result[bonus.Affix] += bonus;
            }
        }

        return result;
    }

    /// <summary>
    /// Collects and applies 4pc drive discs set bonus
    /// </summary>
    /// <param name="fullSets">
    /// A collection of Drive Disc Set IDs for
    /// which 4pc bonus should be applied
    /// </param>
    /// <param name="agent">Agent instance</param>
    /// <returns>A dictionary of all collected stats</returns>
    private SafeDictionary<Affix, double> CollectDriveDiscSetBonus(Agent agent, IEnumerable<uint> fullSets) {
        var result = new SafeDictionary<Affix, double>();
        
        foreach (var set in fullSets) {
            var dds = Resources.Current.GetDriveDiscSet(set);
            
            foreach (var bonus in dds.FullBonus) {
                if (bonus.SkillTags.Length != 0) { 
                    agent.TagBonus.Add(bonus);
                } else
                    result[bonus.Affix] += bonus;
            }
            dds.ApplyPassive?.Invoke(agent);
        }

        return result;
    }*/

    /// <summary>
    /// Main damage calculation function
    /// </summary>
    /// <param name="characterId">Agent ID</param>
    /// <param name="weaponId">Weapon ID</param>
    /// <param name="driveDiscs">Agent's equipped Drive Discs</param>
    /// <param name="team">Team members IDs collection (except current agent)</param>
    /// <param name="rotation">Collection of agent skills</param>
    /// <param name="enemy">Enemy instance</param>
    /// <param name="calcType">Damage or Daze</param>
    /// <returns>A collection of agent actions</returns>
    public CalcResult Calculate(uint characterId, uint weaponId, 
        DriveDisc[] driveDiscs, IEnumerable<uint> team, 
        IEnumerable<string> rotation, Enemy enemy, CalculationType calcType = CalculationType.Damage) {
        
        // Initialize the agent and the weapon
        // Having a dictionary here allows us to use abilities 
        // of other team members
        var fullTeam = new Dictionary<uint, Agent> {
            [characterId] = CreateAgentInstance(characterId)
        };
        
        fullTeam[characterId].SetWeapon(weaponId);
        fullTeam[characterId].SetDriveDiscs(driveDiscs);
        fullTeam[characterId].Hp = fullTeam[characterId].MaxHp;
        // var weapon = Resources.Current.GetWeapon(weaponId);
        //
        // // Collect Drive Discs stats and apply them
        // var groupedSets = driveDiscs
        //     .GroupBy(x => x.SetId)
        //     .ToDictionary(set => set.Key, set => set.Count());
        // var partialSets = groupedSets.Where(kvp => kvp.Value >= 2).Select(kvp => kvp.Key);
        // var fullSets = groupedSets.Where(kvp => kvp.Value >= 4).Select(kvp => kvp.Key);
        //
        // // fullTeam[characterId].BonusStats = CollectDriveDiscStats(driveDiscs, partialSets, fullTeam[characterId].TagBonus);
        //
        // fullTeam[characterId].Stats[Affix.Atk] += weapon.MainStat.Value;
        // fullTeam[characterId].BonusStats[weapon.SecondaryStat.Affix] += weapon.SecondaryStat.Value;
        //
        // var baseStats = fullTeam[characterId].CollectStats();
        
        // var driveDiscSetBonus = CollectDriveDiscSetBonus(fullTeam[characterId], fullSets);
        // foreach (var (afx, val) in driveDiscSetBonus) {
        //     fullTeam[characterId].BonusStats[afx] += val;
        // }
        
        // foreach (var passive in weapon.Passive) {
        //     fullTeam[characterId].BonusStats[passive.Affix] += passive.Value;
        // }
        
        // Apply Agent's passive
        // fullTeam[characterId].ApplyPassive();
        
        // Apply Agent's weapon passive
        // weapon.ApplyPassive?.Invoke(fullTeam[characterId]);
        
        var actions = new List<AgentAction>();
        
        // Apply team passive
        // Those include current agent in the team, because current agent can also
        // have synergy with other team members
        foreach (var member in team) {
            fullTeam[member] = CreateAgentReference(member);
        }
        List<Stat> fullTeamPassive = [];

        List<AgentAction> actionsQueue = [];

        enemy.AttributeAnomalyTrigger = (sender, element, agentId) => {
            var isFrostburnShatter = element == Element.Ice && sender.AfflictedAnomaly?.Element == Element.Frost;

            // Process anomaly damage
            actionsQueue.Add(fullTeam[agentId].GetAnomalyDamage(element, enemy));

            // Then process disorders
            if (sender.AfflictedAnomaly is { } anomaly && !isFrostburnShatter) {
                if (anomaly.Element != element) {
                    actionsQueue.Add(fullTeam[agentId].GetAnomalyDamage(Element.None, enemy));
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

        // Apply Vivian's Action Handler to all team members
        if (fullTeam.TryGetValue(1331, out var agent) && agent is Vivian vivian) {
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
                    actionsQueue.Add(abloom with {
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
            if (actionsQueue.Count > 0) {
                actions.AddRange(actionsQueue);
                actionsQueue.Clear();
            }
        }
        
        // Cleanup actions - remove all damage/daze from other agents
        actions = actions.Select(action => action.AgentId != characterId
            ? action with { Damage = 0, Daze = 0 } 
            : action
        ).ToList();

        var total = calcType switch {
            CalculationType.Damage => actions.Sum(action => action.Damage),
            CalculationType.Daze   => actions.Sum(action => action.Daze),
            _                      => actions.Sum(action => action.Damage) // Fallback to damage
        };
        
        return new CalcResult {
            FinalStats = {
                BaseStats = fullTeam[characterId].BaseStats,
                CalculatedStats = fullTeam[characterId].CollectStats()
            },
            Enemy = enemy,
            PerAction = actions,
            Total = total
        };
    }
}