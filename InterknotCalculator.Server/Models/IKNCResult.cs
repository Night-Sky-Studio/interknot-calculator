using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using MessagePack;

namespace InterknotCalculator.Server.Models;

[MessagePackObject(keyAsPropertyName: true)]
public record EnemyData(
    double Defense,
    double LevelFactor,
    double AnomalyBuildupThreshold,
    Dictionary<Element, Dictionary<uint, double>> AnomalyBuildup,
    double StunMultiplier
);

[MessagePackObject(keyAsPropertyName: true)]
public record ResultData(
    Dictionary<Affix, double> BaseStats,
    Dictionary<Affix, double> CalculatedStats,
    IEnumerable<AgentAction> PerAction,
    double TotalDamage,
    EnemyData Enemy
);

[MessagePackObject(keyAsPropertyName: true)]
public record IKNCResult(
    uint Uid,
    uint LeaderboardId,
    uint BuildId,
    bool IsPrimary,
    ResultData Result
);