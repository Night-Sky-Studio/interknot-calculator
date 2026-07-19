namespace InterknotCalculator.Core.Classes.Weapons;

public static partial class WeaponRegistry {
    private static readonly Dictionary<uint, Func<Weapon>> Instances = new();

    public static Weapon CreateInstance(uint weaponId) {
        if (!Instances.TryGetValue(weaponId, out var instance))
            throw new ArgumentOutOfRangeException(nameof(weaponId), weaponId, 
                $"Weapon {weaponId} is not implemented");
        
        return instance();
    }
}