using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Server.Classes;

public class ResultWriter : IDisposable, IAsyncDisposable {
    private static string ResultFilePath  { get; } = Path.Combine(Path.GetTempPath(), "interknot_calculator.result");

    private Stream Stream { get; } 
    private BinaryWriter Writer { get; } 
    
    public uint Count { get; set; }

    public ResultWriter() {
        Stream = new FileStream(ResultFilePath, FileMode.Create, FileAccess.Write);
        Writer = new(Stream);
        
        Writer.Write("IKNC\0"u8);
        Writer.Write("RES"u8);
        Writer.Write(0xFFFFFFFF); // length placeholder
        Writer.Write(0x00000000); // padding
    }

    public void WriteResult(uint uid, uint leaderboardId, CalcResult result) {
        Count++;
        Writer.Write("CHR"u8);
        Writer.Write(uid);
        Writer.Write(leaderboardId);
        var data = result.Encode();
        Writer.Write((ushort)data.Length);
        Writer.Write(data);
    }

    public string Finish() {
        Stream.Seek(8, SeekOrigin.Begin);
        Writer.Write(Count);
        Writer.Flush();
        return ResultFilePath;
    }
    
    public void Dispose() {
        Stream.Dispose();
        Writer.Dispose();
    }
    public async ValueTask DisposeAsync() {
        await Stream.DisposeAsync();
        await Writer.DisposeAsync();
    }
}