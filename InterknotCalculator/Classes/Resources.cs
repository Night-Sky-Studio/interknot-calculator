using System.Text.Json;

namespace InterknotCalculator.Classes;

public class Resources {
    private bool _isInitialized = false;
    private string WeaponsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "Weapons");
    private string DriveDiscsPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "DriveDiscs");
    private Dictionary<uint, DriveDiscSet> DriveDiscs { get; } = new();
    private Dictionary<uint, Weapon> Weapons { get; } = new();
    
    private string[] GetFilesSafe(string path) {
        if (path == "") return [];
        try {
            return Directory.GetFiles(path);
        } catch (Exception) {
            return [];
        }
    }
    
    public async Task Init() {
        var weapons = GetFilesSafe(WeaponsPath);
        foreach (var weapon in weapons) {
            if (JsonSerializer.Deserialize(await File.ReadAllTextAsync(weapon), SerializerContext.Default.Weapon) is { } json)
                Weapons.Add(uint.Parse(Path.GetFileNameWithoutExtension(weapon)), json);
        }

        var driveDiscs = GetFilesSafe(DriveDiscsPath);
        foreach (var driveDisc in driveDiscs) {
            if (JsonSerializer.Deserialize(await File.ReadAllTextAsync(driveDisc), SerializerContext.Default.DriveDiscSet) is { } json)
                DriveDiscs.Add(uint.Parse(Path.GetFileNameWithoutExtension(driveDisc)), json);
        }

        Console.WriteLine($"Loaded {Weapons.Count} weapons and {DriveDiscs.Count} drive disc sets.");
        _isInitialized = true;
    }

    public static Resources Current = new();

    public Weapon GetWeapon(uint id) {
        if (!_isInitialized)
            throw new Exception("Resources not yet initialized.");
        return Weapons[id];
    }

    public DriveDiscSet GetDriveDiscSet(uint id) {
        if (!_isInitialized)
            throw new Exception("Resources not yet initialized.");
        return DriveDiscs[id];
    }
}