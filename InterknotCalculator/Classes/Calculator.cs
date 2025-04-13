using System.Text.Json;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public class Calculator {
    private string WeaponsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "Weapons");
    private string DriveDiscsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "DriveDiscs");
    private Dictionary<uint, DriveDiscSet> DriveDiscs { get; } = new();
    private Dictionary<uint, Weapon> Weapons { get; } = new();

    private string[] GetFilesSafe(string path) {
        if (path == "") return [];
        try {
            return Directory.GetFiles(path);
        } catch (Exception) {
            return [];
        }
    }

    public async Task Init() {
        var weapons = GetFilesSafe(WeaponsPath);
        foreach (var weapon in weapons) {
            if (JsonSerializer.Deserialize(await File.ReadAllTextAsync(weapon), SerializerContext.Default.Weapon) is { } json)
                Weapons.Add(uint.Parse(Path.GetFileNameWithoutExtension(weapon)), json);
        }

        var driveDiscs = GetFilesSafe(DriveDiscsPath);
        foreach (var driveDisc in driveDiscs) {
            if (JsonSerializer.Deserialize(await File.ReadAllTextAsync(driveDisc), SerializerContext.Default.DriveDiscSet) is { } json)
                DriveDiscs.Add(uint.Parse(Path.GetFileNameWithoutExtension(driveDisc)), json);
        }

        Console.WriteLine($"Loaded {Weapons.Count} weapons and {DriveDiscs.Count} drive disc sets.");
    }

    private static Agent CreateAgentInstance(uint agentId) {
        return agentId switch {
            1041 => new Soldier11(),
            1091 => new Miyabi(),
            1191 => new Ellen(),
            1241 => new ZhuYuan(),
            1261 => new JaneDoe(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent wasn't found.")
        };
    }

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
            var dds = DriveDiscs[set];
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
                break;
            }
        }

        return result;
    }

    private const double DamageTakenMultiplier = 1;

    private static double GetEnemyDefMultiplier(Agent agent) {
        const double enemyDef = 953, levelFactor = 794;
        return levelFactor / (Math.Max(enemyDef * (1 - agent.Stats[Affix.PenRatio])
            - agent.Stats[Affix.Pen], 0) + levelFactor);
    }

    private static AgentAction GetStandardDamage(Agent agent, string skill, int scale, 
        SafeDictionary<SkillTag, Stat> tagDamageBonus, double stunMultiplier = 1d) {
        var data = agent.Skills[skill];
        var attribute = data.Scales[scale].Element ?? agent.Element;
        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(attribute);
        var relatedAffixRes = Helpers.GetRelatedAffixRes(attribute);

        var tagDmgBonus = new SafeDictionary<Affix, double>();
        foreach (var (tag, stat) in tagDamageBonus) {
            if (data.Tag == tag) {
                tagDmgBonus[stat.Affix] += stat.Value;
            }
        }

        var abilityPassive = agent.ApplyAbilityPassive(skill);
        if (abilityPassive is { } passive) {
            tagDmgBonus.Add(passive.Affix, passive.Value);
        }

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

    private static AgentAction GetAnomalyDamage(Agent agent, string anomaly) {
        Anomaly data;
        if (!agent.Anomalies.TryGetValue(anomaly, out data!)) {
            data = Anomaly.GetAnomalyByElement(agent.Element);
        }

        double anomalyCritMultiplier = 1;

        if (data.CanCrit) {
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

    public List<AgentAction> Calculate(uint characterId, uint weaponId, double stunMultiplier, IEnumerable<DriveDisc> driveDiscs, IEnumerable<uint> team, IEnumerable<string> rotation) {
        var agent = CreateAgentInstance(characterId);
        var weapon = Weapons[weaponId];
        var tagDamageBonus = new SafeDictionary<SkillTag, Stat>();

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

        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(agent.Element);
        agent.Stats[relatedAffixDmg] += bonusStats[relatedAffixDmg];
        agent.Stats[Affix.DmgBonus] += bonusStats[Affix.DmgBonus];

        var relatedAffixRes = Helpers.GetRelatedAffixRes(agent.Element);
        agent.Stats[relatedAffixRes] += bonusStats[relatedAffixRes];
        agent.Stats[Affix.ResPen] += bonusStats[Affix.ResPen];

        if (agent.Stats[Affix.CritRate] > 1) {
            agent.Stats[Affix.CritRate] = 1;
        }
        
        agent.ApplyPassive();

        var t = team.Select(CreateAgentInstance).ToList();
        foreach (var stat in agent.ApplyTeamPassive(t)) {
            foreach (var tag in stat.SkillTags) {
                tagDamageBonus.Add(tag, stat);
            }
        }

        var result = new List<AgentAction>();
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
            result.Add(localDmg);
        }

        return result;
    }
}