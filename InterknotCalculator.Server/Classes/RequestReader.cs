using System.Text;

namespace InterknotCalculator.Server.Classes;

public static class BinaryReaderExtensions {
    public static string ReadAsciiString(this BinaryReader reader) {
        using var stream = new MemoryStream();
        byte b;
        
        while ((b = reader.ReadByte()) != 0) {
            stream.WriteByte(b);
        }
        
        return Encoding.ASCII.GetString(stream.GetBuffer(), 0, (int)stream.Length);
    }
    
    public static string ReadString(this BinaryReader reader, int length) {
        return Encoding.ASCII.GetString(reader.ReadBytes(length));
    }
}

public static class RequestReader {
    public record IkncRequest(
        Leaderboard[] Leaderboards,
        Character[] Characters
    );
    
    public record Leaderboard(
        uint Id,
        uint CharacterId,
        uint WeaponId,
        uint[] TeamIds,
        ushort StunMultiplier,
        byte ActionsCount,
        string[] Rotation
    );
    public record DiscStat(byte Affix, byte Level);
    public record DriveDisc(uint SetId, byte Rarity, DiscStat MainStat, DiscStat[] SubStats);
    public record Character(uint Uid, uint CharacterId, IEnumerable<DriveDisc> Discs);

    public static void Read(
        Stream stream,
        Action<IEnumerable<Leaderboard>, Character>? onCharacter = null
    ) {
        using var reader = new BinaryReader(stream);

        var magic = reader.ReadAsciiString();
        if (magic != "IKNC") {
            throw new InvalidDataException("Invalid IKNC file header");
        }
        
        var type = reader.ReadString(3);
        if (type != "REQ") {
            throw new InvalidDataException("Invalid IKNC request type");
        }
        
        uint leaderboardsCount = reader.ReadUInt32();
        uint charactersCount = reader.ReadUInt32();
        
        var leaderboards = new Dictionary<uint, List<Leaderboard>>();

        for (var i = 0; i < leaderboardsCount; i++) {
            magic = reader.ReadString(2);
            if (magic != "LB") {
                throw new InvalidDataException("Invalid IKNC leaderboard header");
            }

            var id = reader.ReadUInt32();
            var characterId = reader.ReadUInt32();
            var weaponId = reader.ReadUInt32();
            uint[] teamIds = [
                reader.ReadUInt32(),
                reader.ReadUInt32()
            ];
            teamIds = teamIds.Where(x => x != 0).ToArray();
            
            var stunMultiplier = reader.ReadUInt16();
            var actionsCount = reader.ReadByte();
            var rotation = new string[actionsCount];
            for (var j = 0; j < actionsCount; j++) {
                rotation[j] = reader.ReadAsciiString();
            }

            if (leaderboards.TryGetValue(characterId, out var existing)) {
                existing.Add(new(
                    id, characterId, weaponId, teamIds, stunMultiplier, actionsCount, rotation    
                ));
            } else {
                leaderboards[characterId] = [new(
                    id, characterId, weaponId, teamIds, stunMultiplier, actionsCount, rotation    
                )];
            }
        }

        for (var i = 0; i < charactersCount; i++) {
            magic = reader.ReadString(3);
            if (magic != "CHR") {
                throw new InvalidDataException("Invalid IKNC character header");
            }
            var uid = reader.ReadUInt32();
            var characterId = reader.ReadUInt32();
            
            var discs = new List<DriveDisc>();
            for (var j = 0; j < 6; j++) {
                var setId = reader.ReadUInt32();
                var rarity = reader.ReadByte();
                var mainStat = new DiscStat(reader.ReadByte(), reader.ReadByte());
                var subStats = new DiscStat[4];
                for (var x = 0; x < 4; x++) {
                    subStats[x] = new(reader.ReadByte(), reader.ReadByte());
                }
                if (setId != 0) {
                    discs.Add(new(setId, rarity, mainStat, subStats));
                }
            }

            onCharacter?.Invoke(leaderboards[characterId], new(uid, characterId, discs));
        }
    }
}