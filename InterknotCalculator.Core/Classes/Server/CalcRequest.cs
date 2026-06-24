using System.Text.Json.Serialization;
using InterknotCalculator.Core.Enums;
using MessagePack;

namespace InterknotCalculator.Core.Classes.Server;

[MessagePackObject(keyAsPropertyName: true)]
public record DriveDiscRequest {
    public uint SetId { get; set; }
    public Rarity Rarity { get; set; }
    public Affix[] Stats { get; set; } = [];
    public uint[] Levels { get; set; } = [];
    
    [JsonIgnore]
    public Dictionary<Affix, uint> StatsLevels => new (Stats.Zip(Levels, (k, v) => new KeyValuePair<Affix, uint>(k ,v)));
}

[MessagePackObject(keyAsPropertyName: true)]
public record TeamMemberRequest(uint AgentId = 0, uint WeaponId = 0, uint DriveDiscSetId = 0) {
    public uint AgentId { get; set; } = AgentId;
    public uint WeaponId { get; set; } = WeaponId;
    public uint DriveDiscSetId { get; set; } = DriveDiscSetId;
}

[MessagePackObject(keyAsPropertyName: true)]
public record CalcRequest {
    public uint AgentId { get; set; }
    public uint WeaponId { get; set; }
    public DriveDiscRequest[] Discs { get; set; } = [];
    public TeamMemberRequest[] Team { get; set; } = [];
    public double StunBonus { get; set; }
    public string[] Rotation { get; set; } = [];
    public uint Mindscape { get; set; }
}