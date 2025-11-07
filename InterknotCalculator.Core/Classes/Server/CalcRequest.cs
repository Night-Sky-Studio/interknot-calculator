using System.Text;
using System.Text.Json.Serialization;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Server;

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

    public static CalcRequest Decode(ReadOnlySpan<byte> data) {
        using var stream = new MemoryStream(data.ToArray());
        using var reader = new BinaryReader(stream);
        var request = new CalcRequest {
            AgentId = reader.ReadUInt16(), 
            WeaponId = reader.ReadUInt16()
        };

        byte discCount = reader.ReadByte();
        request.Discs = new DriveDiscRequest[discCount];
        for (int i = 0; i < discCount; i++) {
            var disc = new DriveDiscRequest {
                SetId = reader.ReadUInt16(), 
                Rarity = (Rarity)reader.ReadByte()
            };
            byte statCount = reader.ReadByte();
            disc.Stats = new Affix[statCount];
            disc.Levels = new uint[statCount];
            for (int j = 0; j < statCount; j++) {
                disc.Stats[j] = (Affix)reader.ReadByte();
                disc.Levels[j] = reader.ReadByte();
            }
            request.Discs[i] = disc;
        }

        byte teamCount = reader.ReadByte();
        request.Team = new uint[teamCount];
        for (int i = 0; i < teamCount; i++) {
            request.Team[i] = reader.ReadUInt16();
        }

        var stunBonus = reader.ReadUInt16() / 1000d;

        var rotationLength = reader.ReadUInt16();
        request.Rotation = new string[rotationLength];
        for (int i = 0; i < rotationLength; i++) {
            byte actionLength = reader.ReadByte();
            request.Rotation[i] = Encoding.ASCII.GetString(reader.ReadBytes(actionLength));
        }

        return request;
    }
}