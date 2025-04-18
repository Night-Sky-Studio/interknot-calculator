using System.Text.Json;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

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
            1191 => new Ellen(),
            1241 => new ZhuYuan(),
            1261 => new JaneDoe(),
            1311 => AstraYao.Reference(), // Support agent - provide reference implementation
            1321 => new Evelyn(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent wasn't found.")
        };
    }

    /// <summary>
    /// Collects all stats from Drive Discs and Drive Discs sets.
    /// </summary>
    /// <param name="driveDiscs">A collection of <see cref="DriveDisc"/> instances</param>
    /// <param name="tagDamageBonus">Tag Damage Bonus dictionary reference</param>
    /// <returns>A dictionary of all collected stats</returns>
    private SafeDictionary<Affix, double> CollectDriveDiscStats(IEnumerable<DriveDisc> driveDiscs, 
        SafeDictionary<SkillTag, Stat> tagDamageBonus) {
        var result = new SafeDictionary<Affix, double>();
        var setCounts = new SafeDictionary<uint, uint>();

        foreach (var disc in driveDiscs) {
            setCounts[disc.SetId] += 1;
            result[disc.MainStat.Affix] += disc.MainStat.Value;
            foreach (var subStat in disc.SubStats) {
                result[subStat.Stat.Affix] += subStat.Level * subStat.Stat.Value;
            }
        }

        foreach (var (set, count) in setCounts) {
            var dds = Resources.Current.GetDriveDiscSet(set);
            if (count >= 2) {
                foreach (var bonus in dds.PartialBonus) {
                    if (bonus.SkillTags.Length != 0) {
                        foreach (var tag in bonus.SkillTags) {
                            tagDamageBonus[tag] = bonus;
                        }
                    } else
                        result[bonus.Affix] += bonus;
                }
            }

            if (count >= 4) {
                foreach (var bonus in dds.FullBonus) {
                    if (bonus.SkillTags.Length != 0) {
                        foreach (var tag in bonus.SkillTags) {
                            tagDamageBonus[tag] = bonus;
                        }
                    } else
                        result[bonus.Affix] += bonus;
                }
            }
        }

        return result;
    }

    /// Default damage multiplier for all attacks
    private const double DamageTakenMultiplier = 1;

    /// <summary>
    /// Calculates enemy defense multiplier
    /// </summary>
    /// <remarks>
    /// Currently, enemy is "Notorious Dullahan" at lvl. 70
    /// </remarks>
    /// <param name="agent">Agent instance</param>
    /// <returns>Enemy Defense multiplier</returns>
    private static double GetEnemyDefMultiplier(Agent agent) {
        const double enemyDef = 953, levelFactor = 794;
        return levelFactor / (Math.Max(enemyDef * (1 - agent.Stats[Affix.PenRatio])
            - agent.Stats[Affix.Pen], 0) + levelFactor);
    }

    /// <summary>
    /// Calculates standard damage for a given agent and skill
    /// </summary>
    /// <param name="agent">Agent instance</param>
    /// <param name="skill">Skill name</param>
    /// <param name="scale">Skill level</param>
    /// <param name="tagDamageBonus">Tag Damage Bonus dictionary reference</param>
    /// <param name="stunMultiplier">Enemy Stun multiplier</param>
    /// <returns><see cref="AgentAction"/> with calculated damage</returns>
    private static AgentAction GetStandardDamage(Agent agent, string skill, int scale, 
        SafeDictionary<SkillTag, Stat> tagDamageBonus, double stunMultiplier = 1d) {
        var data = agent.Skills[skill];
        var attribute = data.Scales[scale].Element ?? agent.Element;
        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(attribute);
        var relatedAffixRes = Helpers.GetRelatedAffixRes(attribute);

        // Process all tag bonuses and apply if tag matches
        var tagDmgBonus = new SafeDictionary<Affix, double>();
        foreach (var (tag, stat) in tagDamageBonus) {
            if (data.Tag == tag) {
                tagDmgBonus[stat.Affix] += stat.Value;
            }
        }

        // Apply ability passive if present
        var abilityPassive = agent.ApplyAbilityPassive(skill);
        if (abilityPassive is { } passive) {
            tagDmgBonus.Add(passive.Affix, passive.Value);
        }

        // Calculate damage according to formula
        var baseDmgAttacker = data.Scales[scale].Damage / 100 * agent.Stats[Affix.Atk];
        var dmgBonusMultiplier = 1 + agent.Stats[relatedAffixDmg] + tagDmgBonus[relatedAffixDmg]
                                 + data.Affixes[Affix.DmgBonus]  + tagDmgBonus[Affix.DmgBonus];
        var critMultiplier = 1 + (agent.Stats[Affix.CritRate] + tagDmgBonus[Affix.CritRate])
            * (agent.Stats[Affix.CritDamage] + tagDmgBonus[Affix.CritDamage]);
        var resMultiplier = 1 + data.Affixes[relatedAffixRes] + tagDmgBonus[relatedAffixRes]
                              + agent.Stats[relatedAffixRes] + agent.Stats[Affix.ResPen]
                              + tagDmgBonus[Affix.ResPen];

        var total = baseDmgAttacker * dmgBonusMultiplier * critMultiplier * GetEnemyDefMultiplier(agent)
            * resMultiplier * DamageTakenMultiplier * stunMultiplier;

        return new() {
            Name = $"{skill} { (scale == 0 && data.Scales.Count == 1 ? "" : scale + 1) }".Trim(),
            Tag = data.Tag,
            Damage = total
        };
    }

    /// <summary>
    /// Gets standard anomaly damage.
    /// </summary>
    /// <param name="agent">Agent instance</param>
    /// <param name="anomaly">Anomaly name</param>
    /// <returns><see cref="AgentAction"/> with calculated damage</returns>
    private static AgentAction GetAnomalyDamage(Agent agent, string anomaly) {
        // Agents can override default anomalies
        // ReSharper disable once InlineOutVariableDeclaration
        Anomaly data;
        if (!agent.Anomalies.TryGetValue(anomaly, out data!)) {
            data = Anomaly.GetAnomalyByElement(agent.Element);
        }

        // Some anomalies (Jane Doe - Assault) can crit
        double anomalyCritMultiplier = 1;

        if (data.CanCrit) {
            // These crit values are not affected by anything other than character's skill kit
            double anomalyCritRate = 0.05, anomalyCritDamage = 0.5;
            foreach (var bonus in data.Bonuses) {
                switch (bonus.Affix) {
                    case Affix.CritRate:
                        anomalyCritRate = Math.Min(bonus.Value, 1);
                        break;
                    case Affix.CritDamage:
                        anomalyCritDamage = bonus.Value;
                        break;
                }
            }

            anomalyCritMultiplier = 1 + anomalyCritRate * anomalyCritDamage;
        }

        var attribute = data.Element;

        // Calculate anomaly damage according to formula
        var anomalyBaseDmg = data.Scale / 100 * agent.Stats[Affix.Atk];
        var anomalyProficiencyMultiplier = agent.Stats[Affix.AnomalyProficiency] / 100;
        const double anomalyLevelMultiplier = 2;
        var dmgBonusMultiplier = 1 + agent.Stats[Helpers.GetRelatedAffixDmg(attribute)] + agent.Stats[Affix.DmgBonus];
        var resMultiplier = 1 + agent.Stats[Helpers.GetRelatedAffixRes(attribute)] + agent.Stats[Affix.ResPen];

        var total = anomalyBaseDmg * anomalyProficiencyMultiplier * anomalyCritMultiplier * anomalyLevelMultiplier
               * dmgBonusMultiplier * GetEnemyDefMultiplier(agent) * resMultiplier;

        return new() {
            Name = anomaly,
            Tag = SkillTag.AttributeAnomaly,
            Damage = total
        };
    }


    /// <summary>
    /// Main damage calculation function
    /// </summary>
    /// <param name="characterId">Agent ID</param>
    /// <param name="weaponId">Weapon ID</param>
    /// <param name="stunMultiplier">Enemy Stun Multiplier</param>
    /// <param name="driveDiscs">Agent's equipped Drive Discs</param>
    /// <param name="team">Team members IDs collection (except current agent)</param>
    /// <param name="rotation">Collection of agent skills/anomalies</param>
    /// <returns>A collection of agent actions</returns>
    public CalcResult Calculate(uint characterId, uint weaponId, 
        double stunMultiplier, IEnumerable<DriveDisc> driveDiscs, IEnumerable<uint> team, 
        IEnumerable<string> rotation) {
        // Initialize the agent and the weapon
        var agent = CreateAgentInstance(characterId);
        var weapon = Resources.Current.GetWeapon(weaponId);
        var tagDamageBonus = new SafeDictionary<SkillTag, Stat>();

        // Collect Drive Discs stats and apply them
        var bonusStats = CollectDriveDiscStats(driveDiscs, tagDamageBonus);

        bonusStats[weapon.SecondaryStat.Affix] += weapon.SecondaryStat.Value;

        foreach (var passive in weapon.Passive) {
            bonusStats[passive.Affix] += passive.Value;
        }

        agent.Stats[Affix.Atk] = (agent.Stats[Affix.Atk] + weapon.MainStat.Value)
            * (1 + bonusStats[Affix.AtkRatio]) + bonusStats[Affix.Atk];

        agent.Stats[Affix.CritRate] += bonusStats[Affix.CritRate];
        agent.Stats[Affix.CritDamage] += bonusStats[Affix.CritDamage];
        agent.Stats[Affix.Pen] += bonusStats[Affix.Pen];
        agent.Stats[Affix.PenRatio] += bonusStats[Affix.PenRatio];

        agent.Stats[Affix.AnomalyProficiency] += bonusStats[Affix.AnomalyProficiency];
        agent.Stats[Affix.AnomalyMastery] += agent.Stats[Affix.AnomalyMastery] * bonusStats[Affix.AnomalyMasteryRatio]
            + bonusStats[Affix.AnomalyMastery];

        // Check for any resistance shred bonuses
        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(agent.Element);
        agent.Stats[relatedAffixDmg] += bonusStats[relatedAffixDmg];
        agent.Stats[Affix.DmgBonus] += bonusStats[Affix.DmgBonus];

        var relatedAffixRes = Helpers.GetRelatedAffixRes(agent.Element);
        agent.Stats[relatedAffixRes] += bonusStats[relatedAffixRes];
        agent.Stats[Affix.ResPen] += bonusStats[Affix.ResPen];
        
        // Apply Agent's passive
        agent.ApplyPassive();
        
        // Cap the Crit Rate
        agent.Stats[Affix.CritRate] = Math.Min(agent.Stats[Affix.CritRate], 1);
        
        // Apply team passive
        // Those include current agent in the team, because current agent can also
        // have synergy with other team members
        List<Agent> fullTeam = [agent ,..team.Select(CreateAgentInstance).ToList()];
        foreach (var stat in agent.ApplyTeamPassive(fullTeam)) {
            foreach (var tag in stat.SkillTags) {
                tagDamageBonus.Add(tag, stat);
            }
        }

        foreach (var a in fullTeam) {
            foreach (var (afx, bonus) in a.ExternalBonus) {
                agent.Stats[afx] += bonus;
            }

            foreach (var (tag, stat) in a.ExternalTagBonus) {
                if (tagDamageBonus.ContainsKey(tag)) {
                    tagDamageBonus[tag] += stat;
                } else {
                    tagDamageBonus[tag] = stat;
                }
            }
        }

        // Do the calculation
        var actions = new List<AgentAction>();
        foreach (var action in rotation) {
            AgentAction localDmg;
            if (agent.Anomalies.ContainsKey(action) || Anomaly.DefaultByNames.ContainsKey(action)) {
                localDmg = GetAnomalyDamage(agent, action);
            } else {
                var attack = action.Split(' ');
                var name = attack[0];
                var idx = attack.Length == 1 ? 1 : int.Parse(attack[1]);

                localDmg = GetStandardDamage(agent, name, idx - 1, tagDamageBonus, stunMultiplier);
            }
            actions.Add(localDmg);
        }

        return new CalcResult {
            FinalStats = agent.Stats,
            PerAction = actions,
            Total = actions.Sum(action => action.Damage)
        };
    }
}