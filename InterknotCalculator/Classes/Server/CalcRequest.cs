using System.Text.Json.Serialization;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Server;

public class DriveDiscRequest {
    [JsonPropertyName("id")]
    public uint SetId { get; set; }
    
    [JsonPropertyName("rarity")]
    public Rarity Rarity { get; set; }

    [JsonPropertyName("stats")] 
    public uint[] Stats { get; set; } = [];
    
    [JsonPropertyName("levels")] 
    public uint[] Levels { get; set; } = [];
}

public class CalcRequest {
    [JsonPropertyName("aid")]
    public uint AgentId { get; set; }
    
    [JsonPropertyName("wid")]
    public uint WeaponId { get; set; }

    [JsonPropertyName("discs")]
    public DriveDiscRequest[] Discs { get; set; } = [];

    [JsonPropertyName("stun_bonus")]
    public double StunBonus { get; set; }
    
    [JsonPropertyName("rotation")]
    public string[] Rotation { get; set; } = [];
}