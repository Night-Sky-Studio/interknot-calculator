using InterknotCalculator.Core.Classes.DriveDiscSets;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Weapons;

#pragma warning disable CS0618 // Type or member is obsolete

namespace InterknotCalculator.Core.Classes.Agents;

/// <summary>
/// Base Support Agent class.
/// 
/// Every agent that can be used as a team member should inherit from this class.
/// </summary>
/// <param name="id">Agent ID</param>
public abstract class SupportAgent(uint id) : Agent(id) {
    protected void SetWeaponPassive(uint weaponId) {
        if (weaponId == 0) return;
        RemoveWeaponStats();
        Weapon = WeaponRegistry.CreateInstance(weaponId);
        AddWeaponStats(true);
    }

    protected void SetDriveDiscsPassive(uint driveDiscSetId, bool partial = false) {
        if (driveDiscSetId == 0) return;
        
        RemoveDiscsStats();
        if (partial) {
            PartialSets.Add(DriveDiscSetRegistry.CreateInstance(driveDiscSetId));
        } else {
            FullSets.Add(DriveDiscSetRegistry.CreateInstance(driveDiscSetId));
        }
        AddDiscsStats(true);
    }
}