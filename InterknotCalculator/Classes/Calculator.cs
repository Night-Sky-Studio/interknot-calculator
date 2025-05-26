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
            1031 => Nicole.Reference(), // Support agent - provide reference implementation
            1041 => new Soldier11(),
            1091 => new Miyabi(),
            1131 => Soukaku.Reference(),
            1151 => Lucy.Reference(),
            1191 => new Ellen(),
            1211 => Rina.Reference(),
            1241 => new ZhuYuan(),
            1261 => new JaneDoe(),
            1311 => AstraYao.Reference(),
            1321 => new Evelyn(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent wasn't found.")
        };
    }

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
    /// <param name="tagDamageBonus">Tag Damage Bonus dictionary reference</param>
    /// <returns>A dictionary of all collected stats</returns>
    private SafeDictionary<Affix, double> CollectDriveDiscSetBonus(IEnumerable<uint> fullSets, 
        List<Stat> tagDamageBonus) {
        var result = new SafeDictionary<Affix, double>();
        
        foreach (var set in fullSets) {
            var dds = Resources.Current.GetDriveDiscSet(set);
            
            foreach (var bonus in dds.FullBonus) {
                if (bonus.SkillTags.Length != 0) { 
                    tagDamageBonus.Add(bonus);
                } else
                    result[bonus.Affix] += bonus;
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
        return levelFactor / (Math.Max(enemyDef * (1 - agent.PenRatio) - agent.Pen, 0) + levelFactor);
    }

    /// <summary>
    /// Calculates standard damage for a given agent and skill
    /// </summary>
    /// <param name="agent">Agent instance</param>
    /// <param name="skill">Skill name</param>
    /// <param name="scale">Skill level</param>
    /// <param name="stunMultiplier">Enemy Stun multiplier</param>
    /// <returns><see cref="AgentAction"/> with calculated damage</returns>
    private static AgentAction GetStandardDamage(Agent agent, string skill, int scale, double stunMultiplier = 1d) {
        var data = agent.Skills[skill];
        var attribute = data.Scales[scale].Element ?? agent.Element;
        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(attribute);
        var relatedAffixRes = Helpers.GetRelatedAffixRes(attribute);

        // Process all tag bonuses and apply if tag matches
        var tagDmgBonus = new SafeDictionary<Affix, double>();
        foreach (var stat in agent.TagBonus) {
            if (stat.SkillTags.Contains(data.Tag)) {
                tagDmgBonus[stat.Affix] += stat.Value;
            }
        }

        // Apply ability passive if present
        var abilityPassive = agent.ApplyAbilityPassive(skill);
        if (abilityPassive is { } passive) {
            tagDmgBonus.Add(passive.Affix, passive.Value);
        }

        // Calculate damage according to formula
        var baseDmgAttacker = data.Scales[scale].Damage / 100 * agent.Atk;
        var dmgBonusMultiplier = 1 + agent.ElementalDmgBonus + agent.DmgBonus 
                                 + tagDmgBonus[relatedAffixDmg] + tagDmgBonus[Affix.DmgBonus]
                                 + data.Affixes[relatedAffixDmg] + data.Affixes[Affix.DmgBonus];
        var critMultiplier = 1 + Math.Min(agent.CritRate + tagDmgBonus[Affix.CritRate] + data.Affixes[Affix.CritRate], 1)
            * (agent.CritDamage + tagDmgBonus[Affix.CritDamage] + data.Affixes[Affix.CritDamage]);
        var resMultiplier = 1 + agent.ElementalResPen + agent.ResPen 
                            + tagDmgBonus[relatedAffixRes] + tagDmgBonus[Affix.ResPen]
                            + data.Affixes[relatedAffixRes] + data.Affixes[Affix.ResPen];

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

        // Calculate anomaly damage according to formula
        var anomalyBaseDmg = data.Scale / 100 * agent.Atk;
        var anomalyProficiencyMultiplier = agent.AnomalyProficiency / 100;
        const double anomalyLevelMultiplier = 2;
        var dmgBonusMultiplier = 1 + agent.ElementalDmgBonus + agent.DmgBonus;
        var resMultiplier = 1 + agent.ElementalResPen + agent.ResPen;

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
        double stunMultiplier, List<DriveDisc> driveDiscs, IEnumerable<uint> team, 
        IEnumerable<string> rotation) {
        // Initialize the agent and the weapon
        var agent = CreateAgentInstance(characterId);
        var weapon = Resources.Current.GetWeapon(weaponId);

        // Collect Drive Discs stats and apply them
        var groupedSets = driveDiscs
            .GroupBy(x => x.SetId)
            .ToDictionary(set => set.Key, set => set.Count());
        var partialSets = groupedSets.Where(kvp => kvp.Value >= 2).Select(kvp => kvp.Key);
        var fullSets = groupedSets.Where(kvp => kvp.Value >= 4).Select(kvp => kvp.Key);
        
        agent.BonusStats = CollectDriveDiscStats(driveDiscs, partialSets, agent.TagBonus);
        
        agent.Stats[Affix.Atk] += weapon.MainStat.Value;
        agent.BonusStats[weapon.SecondaryStat.Affix] += weapon.SecondaryStat.Value;

        var baseStats = agent.CollectStats();
        
        var driveDiscSetBonus = CollectDriveDiscSetBonus(fullSets, agent.TagBonus);
        foreach (var (afx, val) in driveDiscSetBonus) {
            agent.BonusStats[afx] += val;
        }
        
        foreach (var passive in weapon.Passive) {
            agent.BonusStats[passive.Affix] += passive.Value;
        }
        
        // Apply Agent's passive
        agent.ApplyPassive();
        
        // Apply team passive
        // Those include current agent in the team, because current agent can also
        // have synergy with other team members
        List<Agent> fullTeam = [agent ,..team.Select(CreateAgentInstance).ToList()];
        List<Stat> fullTeamPassive = [];
        
        // All supports are rocking "Astral Voice" set
        // they need to be counted to then subtract excess applications of this set
        // as "Astral Voice" 4pc bonus can only be applied once
        var astralVoiceCount = fullTeam.Count(a => a.Speciality == Speciality.Support);
        foreach (var a in fullTeam) {
            fullTeamPassive.AddRange(a.ApplyTeamPassive(fullTeam));
        }
        
        foreach (var stat in fullTeamPassive) {
            if (stat.SkillTags.Length > 0) {
                agent.TagBonus.Add(stat);
            } else {
                agent.BonusStats[stat.Affix] += stat.Value;
            }
        }

        foreach (var a in fullTeam) {
            foreach (var (afx, bonus) in a.ExternalBonus) {
                agent.BonusStats[afx] += bonus;
            }

            foreach (var stat in a.ExternalTagBonus) {
                agent.TagBonus.Add(stat);
            }
        }
        
        // Subtract excess applications of "Astral Voice" 4pc bonus
        // I can't believe how hacky this feels...
        if (astralVoiceCount > 1) {
            var astralVoice = Resources.Current.GetDriveDiscSet(32800);
            var avBonus = astralVoice.FullBonus.First();
            agent.BonusStats[avBonus.Affix] -= avBonus.Value * (astralVoiceCount - 1);
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

                localDmg = GetStandardDamage(agent, name, idx - 1, stunMultiplier);
            }
            actions.Add(localDmg);
        }

        return new CalcResult {
            FinalStats = {
                BaseStats = baseStats,
                CalculatedStats = agent.CollectStats()
            },
            PerAction = actions,
            Total = actions.Sum(action => action.Damage)
        };
    }
}