using InterknotCalculator.Core.Enums;
using MessagePack;

namespace InterknotCalculator.Server.Models;

[MessagePackObject]
public record DiscStat(
    [property: Key(0)] byte Affix, 
    [property: Key(1)] byte Level
);

[MessagePackObject]
public record DriveDisc(
    [property: Key(0)] uint SetId, 
    [property: Key(1)] Rarity Rarity,
    [property: Key(2)] DiscStat MainStat, 
    [property: Key(3)] IEnumerable<DiscStat> SubStats
);

[MessagePackObject]
public record Character(
    [property: Key(0)] uint Uid, 
    [property: Key(1)] uint BuildId, 
    [property: Key(2)] bool IsPrimary, 
    [property: Key(3)] uint CharacterId,
    [property: Key(4)] IEnumerable<DriveDisc> Discs
);

[MessagePackObject]
public record TeamMember(
    [property: Key(0)] uint AgentId, 
    [property: Key(1)] uint WeaponId, 
    [property: Key(2)] uint DriveDiscSetId
);

[MessagePackObject]
public record Leaderboard(
    [property: Key(0)] uint Id,
    [property: Key(1)] uint CharacterId,
    [property: Key(2)] uint WeaponId,
    [property: Key(3)] IEnumerable<TeamMember> Team,
    [property: Key(4)] byte Mindscape,
    [property: Key(5)] double StunMultiplier,
    [property: Key(6)] IEnumerable<string> Rotation
);
