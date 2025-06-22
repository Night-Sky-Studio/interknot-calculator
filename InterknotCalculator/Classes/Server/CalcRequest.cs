using System.Text.Json.Serialization;
using InterknotCalculator.Classes.Enemies;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Server;

public class DriveDiscRequest {
    [JsonPropertyName("id")]
    public uint SetId { get; set; }
    
    [JsonPropertyName("rarity")]
    public Rarity Rarity { get; set; }

    [JsonPropertyName("stats")] 
    public Affix[] Stats { get; set; } = [];
    
    [JsonPropertyName("levels")] 
    public uint[] Levels { get; set; } = [];
    
    [JsonIgnore]
    public Dictionary<Affix, uint> StatsLevels => new (Stats.Zip(Levels, (k, v) => new KeyValuePair<Affix, uint>(k ,v)));
}

public class CalcRequest {
    [JsonPropertyName("aid")]
    public uint AgentId { get; set; }
    
    [JsonPropertyName("wid")]
    public uint WeaponId { get; set; }

    [JsonPropertyName("discs")]
    public DriveDiscRequest[] Discs { get; set; } = [];
    
    [JsonPropertyName("team")]
    public uint[] Team { get; set; } = [];

    [JsonPropertyName("stun_bonus")]
    public double StunBonus { get; set; }
    
    [JsonPropertyName("rotation")]
    public string[] Rotation { get; set; } = [];
    
    [JsonPropertyName("enemy")]
    public Enemy? Enemy { get; set; }
}