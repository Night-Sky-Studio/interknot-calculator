using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Extensions;
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

    private Agent CreateAgentInstance(uint agentId) {
        return agentId switch {
            1091 => new Miyabi(),
            1241 => new ZhuYuan(),
            1261 => new JaneDoe(),
            _ => throw new ArgumentOutOfRangeException(nameof(agentId), agentId, "Agent wasn't found.")
        };
    }
    [NotNull]
    private Agent? Agent { get; set;  }
    private uint WeaponId { get; set; }
    private Weapon Weapon => Weapons[WeaponId];

    private SafeDictionary<Affix, double> CollectDriveDiscStats(IEnumerable<DriveDisc> driveDiscs) {
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
                            TagDamageBonus[tag] = bonus;
                        }
                    } else 
                        result[bonus.Affix] += bonus;
                }
            }

            if (count >= 4) {
                foreach (var bonus in dds.FullBonus) {
                    if (bonus.SkillTags.Length != 0) {
                        foreach (var tag in bonus.SkillTags) {
                            TagDamageBonus[tag] = bonus;
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
    private const double StunMultiplier = 1;
    
    private SafeDictionary<SkillTag, Stat> TagDamageBonus { get; } = new();
    
    public void Reset() {
        Agent = null;
        TagDamageBonus.Clear();
    }
    
    private double GetEnemyDefMultiplier() {
        const double enemyDef = 953, levelFactor = 794;
        return levelFactor / (Math.Max(enemyDef * (1 - Agent.Stats[Affix.PenRatio])
            - Agent.Stats[Affix.Pen], 0) + levelFactor);
    }

    private AgentAction GetStandardDamage(string skill, int scale) {
        var data = Agent.Skills[skill];
        var attribute = data.Scales[scale].Element ?? Agent.Element;
        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(attribute);
        var relatedAffixRes = Helpers.GetRelatedAffixRes(attribute);

        var tagDmgBonus = new SafeDictionary<Affix, double>();
        foreach (var (tag, stat) in TagDamageBonus) {
            if (data.Tag == tag) {
                tagDmgBonus[stat.Affix] += stat.Value;
            }
        }
        
        var baseDmgAttacker = data.Scales[scale].Damage / 100 * Agent.Stats[Affix.Atk];
        var dmgBonusMultiplier = 1 + Agent.Stats[relatedAffixDmg] + tagDmgBonus[relatedAffixDmg] 
                                 + data.Affixes[Affix.DmgBonus]  + tagDmgBonus[Affix.DmgBonus];
        var critMultiplier = 1 + (Agent.Stats[Affix.CritRate] + tagDmgBonus[Affix.CritRate]) 
            * (Agent.Stats[Affix.CritDamage] + tagDmgBonus[Affix.CritDamage]);
        var resMultiplier = 1 + data.Affixes[relatedAffixRes] + tagDmgBonus[relatedAffixRes]
                              + Agent.Stats[relatedAffixRes] + Agent.Stats[Affix.ResPen] 
                              + tagDmgBonus[Affix.ResPen];
        
        var total = baseDmgAttacker * dmgBonusMultiplier * critMultiplier * GetEnemyDefMultiplier() 
            * resMultiplier * DamageTakenMultiplier * StunMultiplier;
        
        return new() {
            Name = $"{skill} { (scale == 0 && data.Scales.Count == 1 ? "" : scale + 1) }".Trim(),
            Tag = data.Tag,
            Damage = total
        };
    }

    private AgentAction GetAnomalyDamage(string anomaly) {
        Anomaly data;
        if (!Agent.Anomalies.TryGetValue(anomaly, out data!)) {
            data = Anomaly.GetAnomalyByElement(Agent.Element);
        }

        double anomalyCritMultiplier = 1;
        
        if (data.CanCrit) {
            double anomalyCritRate = 0.05, anomalyCritDamage = 0.5;
            foreach (var bonus in data.Bonuses) {
                switch (bonus.Affix) {
                    case Affix.CritRate:
                        anomalyCritRate = bonus.Value;
                        break;
                    case Affix.CritDamage:
                        anomalyCritDamage = bonus.Value;
                        break;
                }
            }
            
            anomalyCritMultiplier = 1 + anomalyCritRate * anomalyCritDamage;
        }

        var attribute = data.Element;

        var anomalyBaseDmg = data.Scale / 100 * Agent.Stats[Affix.Atk];
        var anomalyProficiencyMultiplier = Agent.Stats[Affix.AnomalyProficiency] / 100;
        const double anomalyLevelMultiplier = 2;
        var dmgBonusMultiplier = 1 + Agent.Stats[Helpers.GetRelatedAffixDmg(attribute)] + Agent.Stats[Affix.DmgBonus];
        var resMultiplier = 1 + Agent.Stats[Helpers.GetRelatedAffixRes(attribute)] + Agent.Stats[Affix.ResPen];

        var total = anomalyBaseDmg * anomalyProficiencyMultiplier * anomalyCritMultiplier * anomalyLevelMultiplier 
               * dmgBonusMultiplier * GetEnemyDefMultiplier() * resMultiplier;

        return new() {
            Name = anomaly,
            Tag = SkillTag.AttributeAnomaly,
            Damage = total
        };
    }
    
    public List<AgentAction> Calculate(uint characterId, uint weaponId, IEnumerable<DriveDisc> driveDiscs, IEnumerable<string> rotation) {
        Agent = CreateAgentInstance(characterId);
        WeaponId = weaponId;
        
        var bonusStats = CollectDriveDiscStats(driveDiscs);
        
        bonusStats[Weapon.SecondaryStat.Affix] += Weapon.SecondaryStat.Value;

        foreach (var passive in Weapon.Passive) {
            bonusStats[passive.Affix] += passive.Value;
        }

        Agent.Stats[Affix.Atk] = (Agent.Stats[Affix.Atk] + Weapon.MainStat.Value) 
            * (1 + bonusStats[Affix.AtkRatio]) + bonusStats[Affix.Atk];
        
        Agent.Stats[Affix.CritRate] += bonusStats[Affix.CritRate];
        Agent.Stats[Affix.CritDamage] += bonusStats[Affix.CritDamage];
        Agent.Stats[Affix.Pen] += bonusStats[Affix.Pen];
        Agent.Stats[Affix.PenRatio] += bonusStats[Affix.PenRatio];
        
        Agent.Stats[Affix.AnomalyProficiency] += bonusStats[Affix.AnomalyProficiency];
        Agent.Stats[Affix.AnomalyMastery] += Agent.Stats[Affix.AnomalyMastery] * bonusStats[Affix.AnomalyMasteryRatio] 
            + bonusStats[Affix.AnomalyMastery];

        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(Agent.Element);
        Agent.Stats[relatedAffixDmg] += bonusStats[relatedAffixDmg];
        Agent.Stats[Affix.DmgBonus] += bonusStats[Affix.DmgBonus];
        
        var relatedAffixRes = Helpers.GetRelatedAffixRes(Agent.Element);
        Agent.Stats[relatedAffixRes] += bonusStats[relatedAffixRes];
        Agent.Stats[Affix.ResPen] += bonusStats[Affix.ResPen];
        
        Agent.ApplyPassive();
        
        StringExtensions.Variables = new() {
            { "Atk", Agent.Stats[Affix.Atk] },
            { "CritRate", Agent.Stats[Affix.CritRate] },
            { "CritDamage", Agent.Stats[Affix.CritDamage] },
            { "Pen", Agent.Stats[Affix.Pen] },
            { "PenRatio", Agent.Stats[Affix.PenRatio] },
            { "AnomalyProficiency", Agent.Stats[Affix.AnomalyProficiency] },
            { "AnomalyMastery", Agent.Stats[Affix.AnomalyMastery] }
        };
        
        var result = new List<AgentAction>();
        foreach (var action in rotation) {
            AgentAction localDmg;
            if (Agent.Anomalies.ContainsKey(action) || Anomaly.DefaultByNames.ContainsKey(action)) {
                localDmg = GetAnomalyDamage(action);
            } else {
                var attack = action.Split(' ');
                var name = attack[0];
                var idx = attack.Length == 1 ? 1 : int.Parse(attack[1]);

                localDmg = GetStandardDamage(name, idx - 1);
            }
            result.Add(localDmg);
        }
        
        return result;
    }
}