using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using MessagePack;

namespace InterknotCalculator.Server.Models;

[MessagePackObject(keyAsPropertyName: true)]
public record DiscStat(byte Affix, byte Level);

[MessagePackObject(keyAsPropertyName: true)]
public record DriveDisc(
    uint SetId, 
    Rarity Rarity,
    DiscStat MainStat, 
    IEnumerable<DiscStat> SubStats
);

[MessagePackObject(keyAsPropertyName: true)]
public record Character(
    uint Uid, 
    uint BuildId, 
    bool IsPrimary, 
    uint CharacterId, 
    uint Mindscape,
    IEnumerable<DriveDisc> Discs
);

[MessagePackObject(keyAsPropertyName: true)]
public record TeamMember(uint AgentId, uint WeaponId, uint DriveDiscSetId);

[MessagePackObject(keyAsPropertyName: true)]
public record Leaderboard(
    uint Id,
    uint CharacterId,
    uint WeaponId,
    IEnumerable<TeamMember> Team,
    ushort StunMultiplier,
    byte ActionsCount,
    IEnumerable<string> Rotation
);

[MessagePackObject(keyAsPropertyName: true)]
public record IKNCRequest(
    IEnumerable<Leaderboard> Leaderboards,
    IEnumerable<Character> Characters
);
