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
public record TeamMemberRequest {
    public uint AgentId { get; set; }
    public uint WeaponId { get; set; }
    public uint DriveDiscSetId { get; set; }
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