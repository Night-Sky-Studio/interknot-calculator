using System.Text.Json;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

/// <summary>
/// Calculator Resource Manager
/// Provides global access to loaded Weapons and Drive Discs
/// </summary>
public class Resources {
    private bool _isInitialized = false;
    private string WeaponsPath { get; } = Path.Combine(AppContext.BaseDirectory, "Resources", "Weapons");
    private string DriveDiscsPath { get; } = Path.Combine(AppContext.BaseDirectory, "Resources", "DriveDiscs");
    private Dictionary<uint, DriveDiscSet> DriveDiscs { get; } = new();
    private Dictionary<uint, Weapon> Weapons { get; } = new();
    
    /// <summary>
    /// <inheritdoc cref="Directory.GetFiles(string)"/>
    /// Does not throw.
    /// </summary>
    /// <param name="path"><inheritdoc cref="Directory.GetFiles(string)"/></param>
    /// <returns><inheritdoc cref="Directory.GetFiles(string)"/></returns>
    private string[] GetFilesSafe(string path) {
        if (path == "") return [];
        try {
            return Directory.GetFiles(path);
        } catch (Exception) {
            return [];
        }
    }
    
    /// <summary>
    /// Initializes resource manager.
    /// Loads Weapons and Drive Discs definitions from <see cref="WeaponsPath"/> and <see cref="DriveDiscsPath"/> 
    /// </summary>
    public async Task Init() {
        if (_isInitialized) return;
        
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

    /// <summary>
    /// Current instance of Resource Manager
    /// </summary>
    public static readonly Resources Current = new();

    /// <summary>
    /// Returns weapon data from loaded weapons
    /// </summary>
    /// <param name="id">Weapon ID</param>
    /// <returns><see cref="Weapon"/> instance</returns>
    /// <exception cref="Exception">Manager not initialized</exception>
    /// <exception cref="KeyNotFoundException">Weapon is not implemented</exception>
    public Weapon GetWeapon(uint id) {
        if (!_isInitialized)
            throw new Exception("Resources not yet initialized.");

        if (!Weapons.TryGetValue(id, out var weapon))
            throw new KeyNotFoundException($"Weapon {id} not found");
        
        weapon.ApplyPassive = id switch {
            14122 => agent => {
                agent.BonusStats[Affix.DisorderDmgBonus] += agent.AnomalyProficiency > 375 ? 0.25 : 0;
            },
            14133 => agent => {
                if (agent.Element is Element.Ether) {
                    agent.BonusStats[Affix.AnomalyProficiency] += 20 * 6;
                }
            },
            _ => weapon.ApplyPassive
        };

        return weapon;
    }

    /// <summary>
    /// Returns drive disc set data from loaded sets
    /// </summary>
    /// <param name="id">Drive Disc Set ID</param>
    /// <returns><see cref="DriveDiscSet"/> instance</returns>
    /// <exception cref="Exception">Manager not initialized</exception>
    /// <exception cref="KeyNotFoundException">Drive Discs Set is not implemented</exception>
    public DriveDiscSet GetDriveDiscSet(uint id) {
        if (!_isInitialized)
            throw new Exception("Resources not yet initialized.");

        if (!DriveDiscs.TryGetValue(id, out DriveDiscSet? set))
            throw new KeyNotFoundException($"Drive Disc Set {id} not found");
        
        set.ApplyPassive = id switch {
            DriveDiscSetId.DawnsBloom => agent => {
                agent.TagBonus.Add(new(Affix.DmgBonus, agent.Speciality == Speciality.Attack ? 0.4 : 0.2,  tags: [SkillTag.BasicAtk]));
            },
            DriveDiscSetId.MoonlightLullaby => agent => {
                if (agent.Speciality == Speciality.Support) {
                    agent.ExternalBonus[Affix.DmgBonus] += 0.18;
                }
            },
            _ => set.ApplyPassive
        };
        
        return set;
    }
}