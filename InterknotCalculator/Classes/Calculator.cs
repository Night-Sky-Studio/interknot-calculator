﻿using System.Text.Json;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Extensions;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public class Calculator {
    private string AgentsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "Agents");
    private string WeaponsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "Weapons");
    private string DriveDiscsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "DriveDiscs");

    private Dictionary<uint, Agent> Agents { get; set; } = new();
    private Dictionary<uint, DriveDiscSet> DriveDiscs { get; set; } = new();
    private Dictionary<uint, Weapon> Weapons { get; set; } = new();

    private string[] GetFilesSafe(string path) {
        if (path == "") return [];
        try {
            return Directory.GetFiles(path);
        } catch (Exception) {
            return [];
        }
    }
    
    public async Task Init() {
        var agents = GetFilesSafe(AgentsPath);
        foreach (var agent in agents) {
            if (JsonSerializer.Deserialize(await File.ReadAllTextAsync(agent), SerializerContext.Default.Agent) is { } json)
                Agents.Add(uint.Parse(Path.GetFileNameWithoutExtension(agent)), json);
        }
        
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
        
        Console.WriteLine($"Loaded {Agents.Count} agents, {Weapons.Count} weapons and {DriveDiscs.Count} drive disc sets.");
    }

    private uint AgentId { get; set; }
    private Agent Agent => Agents[AgentId];

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
                    result[bonus.Affix] += bonus.Value;
                }
            }

            if (count >= 4) {
                foreach (var bonus in dds.FullBonus) {
                    result[bonus.Affix] += bonus.Value;
                }
                break;
            }
        }
        
        
        return result;
    }

    private const double DamageTakenMultiplier = 1;
    private const double StunMultiplier = 1;
    
    private double TotalAtk { get; set; } 
    private double CritRate { get; set; } 
    private double CritDamage { get; set; } 
    private double Pen { get; set; } 
    private double PenRatio { get; set; } 
    private double AnomalyProficiency { get; set; }
    private double AnomalyMastery { get; set; }
    private Dictionary<Affix, double> AttributeDmgBonus { get; } = new();
    private Dictionary<Affix, double> AttributeDmgRes { get; } = new();

    public void Reset() {
        TotalAtk = 0;
        CritRate = 0;
        CritDamage = 0;
        Pen = 0;
        PenRatio = 0;
        AnomalyProficiency = 0;
        AnomalyMastery = 0;
        AttributeDmgBonus.Clear();
        AttributeDmgRes.Clear();
    }
    
    private double GetEnemyDefMultiplier() {
        const double enemyDef = 953, levelFactor = 794;
        return levelFactor / (Math.Max(enemyDef * (1 - PenRatio) - Pen, 0) + levelFactor);
    }

    private double GetStandardDamage(string skill, Index scale) {
        var data = Agent.Skills[skill];
        var attribute = data.Scales[scale].Element ?? Agent.Element;

        var baseDmgAttacker = data.Scales[scale].Damage / 100 * TotalAtk;
        var dmgBonusMultiplier = 1 + AttributeDmgBonus[Helpers.GetRelatedAffixDmg(attribute)] + data.Affixes[Affix.DmgBonus];
        var critMultiplier = 1 + CritRate * CritDamage;
        var resMultiplier = 1 + data.Affixes[Helpers.GetRelatedAffixRes(attribute)] 
                              + AttributeDmgRes[Helpers.GetRelatedAffixRes(attribute)] + AttributeDmgRes[Affix.ResPen];

        return baseDmgAttacker * dmgBonusMultiplier * critMultiplier * GetEnemyDefMultiplier() * resMultiplier *
               DamageTakenMultiplier * StunMultiplier;
    }

    private double GetAnomalyDamage(string anomaly) {
        Anomaly data;
        if (!Agent.Anomalies.TryGetValue(anomaly, out data!)) {
            data = Anomaly.GetAnomalyByElement(Agent.Element);
        }

        double anomalyCritMultiplier = 1;
        
        if (data.CanCrit) {
            double anomalyCritRate = 0.05, anomalyCritDamage = 0.5;
            foreach (var bonus in data.Bonuses) {
                if (bonus is { Expression: not null, Value: 0 }) {
                    bonus.Value = bonus.Expression.EvaluateExpression(ExpressionParams);
                }

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

        var anomalyBaseDmg = data.Scale / 100 * TotalAtk;
        var anomalyProficiencyMultiplier = AnomalyProficiency / 100;
        const double anomalyLevelMultiplier = 2;
        var dmgBonusMultiplier = 1 + AttributeDmgBonus[Helpers.GetRelatedAffixDmg(attribute)] + AttributeDmgBonus[Affix.DmgBonus];
        var resMultiplier = 1 + AttributeDmgRes[Helpers.GetRelatedAffixRes(attribute)] + AttributeDmgRes[Affix.ResPen];

        return anomalyBaseDmg * anomalyProficiencyMultiplier * anomalyCritMultiplier * anomalyLevelMultiplier 
               * dmgBonusMultiplier * GetEnemyDefMultiplier() * resMultiplier;
    }
    
    private Dictionary<string, double> ExpressionParams => new() {
        { "Atk", TotalAtk },
        { "CritRate", CritRate },
        { "CritDamage", CritDamage },
        { "Pen", Pen },
        { "PenRatio", PenRatio },
        { "AnomalyProficiency", AnomalyProficiency },
        { "AnomalyMastery", AnomalyMastery }
    };
    
    public (IEnumerable<double> PerAction, double Total) Calculate(uint characterId, uint weaponId, IEnumerable<DriveDisc> driveDiscs, IEnumerable<string> rotation) {
        AgentId = characterId;
        WeaponId = weaponId;
        
        var bonusStats = CollectDriveDiscStats(driveDiscs);
        
        bonusStats[Weapon.SecondaryStat.Affix] += Weapon.SecondaryStat.Value;

        foreach (var passive in Weapon.Passive) {
            bonusStats[passive.Affix] += passive.Value;
        }

        TotalAtk = (Agent.Stats[Affix.Atk] + Weapon.MainStat.Value) 
            * (1 + bonusStats[Affix.AtkRatio]) + bonusStats[Affix.Atk];
        
        CritRate = Agent.Stats[Affix.CritRate] + bonusStats[Affix.CritRate];
        CritDamage = Agent.Stats[Affix.CritDamage] + bonusStats[Affix.CritDamage];
        Pen = Agent.Stats[Affix.Pen] + bonusStats[Affix.Pen];
        PenRatio = Agent.Stats[Affix.PenRatio] + bonusStats[Affix.PenRatio];
        
        AnomalyProficiency = Agent.Stats[Affix.AnomalyProficiency] + bonusStats[Affix.AnomalyProficiency];
        AnomalyMastery = Agent.Stats[Affix.AnomalyMastery] 
                         + 1 * bonusStats[Affix.AnomalyMasteryRatio] + bonusStats[Affix.AnomalyMastery];

        var relatedAffixDmg = Helpers.GetRelatedAffixDmg(Agent.Element);
        AttributeDmgBonus[relatedAffixDmg] = Agent.Stats[relatedAffixDmg] + bonusStats[relatedAffixDmg];
        AttributeDmgBonus[Affix.DmgBonus] = Agent.Stats[Affix.DmgBonus] + bonusStats[Affix.DmgBonus];
        
        var relatedAffixRes = Helpers.GetRelatedAffixRes(Agent.Element);
        AttributeDmgRes[relatedAffixRes] = Agent.Stats[relatedAffixRes] + bonusStats[relatedAffixRes];
        AttributeDmgRes[Affix.ResPen] = Agent.Stats[Affix.ResPen] + bonusStats[Affix.ResPen];
        
        var result = new List<double>();
        var total = 0d;
        foreach (var action in rotation) {
            var localDmg = 0d;
            if (Agent.Anomalies.ContainsKey(action) || Anomaly.DefaultByNames.ContainsKey(action)) {
                localDmg += GetAnomalyDamage(action);
            } else {
                var attack = action.Split(' ');
                var name = attack[0];
                var idx = attack.Length == 1 ? 1 : int.Parse(attack[1]);

                localDmg += GetStandardDamage(name, idx - 1);
            }
            result.Add(localDmg);
            total += localDmg;
        }
        
        return (result, total);
    }
}