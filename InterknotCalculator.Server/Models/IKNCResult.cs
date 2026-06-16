using InterknotCalculator.Core.Enums;
using MessagePack;

namespace InterknotCalculator.Server.Models;

[MessagePackObject]
public record EnemyData(
    // [property: Key(0)] double Defense,
    // [property: Key(1)] double LevelFactor,
    [property: Key(0)] double AnomalyBuildupThreshold,
    [property: Key(1)] Dictionary<Element, Dictionary<uint, double>> AnomalyBuildup
    // [property: Key(4)] double StunMultiplier
);

[MessagePackObject]
public record AgentAction(
    [property: Key(0)] uint AgentId, 
    [property: Key(1)] string Name, 
    [property: Key(2)] SkillTag Tag, 
    [property: Key(3)] double Damage, 
    [property: Key(4)] double Daze
);

[MessagePackObject]
public record ResultData(
    [property: Key(0)] Dictionary<Affix, double> BaseStats,
    [property: Key(1)] Dictionary<Affix, double> CalculatedStats,
    [property: Key(2)] IEnumerable<AgentAction> PerAction,
    [property: Key(3)] double TotalDamage
    // [property: Key(4)] EnemyData Enemy
);

[MessagePackObject]
public record IKNCResult(
    [property: Key(0)] uint Uid,
    [property: Key(1)] uint LeaderboardId,
    [property: Key(2)] uint BuildId,
    [property: Key(3)] bool IsPrimary,
    [property: Key(4)] ResultData Result
);