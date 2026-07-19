namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public static partial class DriveDiscSetRegistry {
    private static readonly Dictionary<uint, Func<DriveDiscSet>> Instances = new();

    public static DriveDiscSet CreateInstance(uint ddsId) {
        if (!Instances.TryGetValue(ddsId, out var instance))
            throw new ArgumentOutOfRangeException(nameof(ddsId), ddsId, 
                $"Drive Disc Set {ddsId} is not implemented");
        
        return instance();
    }
}