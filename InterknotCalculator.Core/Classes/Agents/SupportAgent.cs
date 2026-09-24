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
        RemoveWeaponPassive();
        if (weaponId == 0) return;

        var weapon = WeaponRegistry.CreateInstance(weaponId);
        if (weapon.Speciality != Speciality) return;

        // Only the passive is applied here, keyed the same way Agent.AddWeaponStats keys it,
        // so it can be removed cleanly and so tagged passives route through MutableStat.Tagged.
        foreach (var passive in weapon.Passive) {
            Stats[passive.Affix].Add(new(ModifierKey.Weapon(weapon.Id) + ModifierKey.CorePassive(), passive));
        }
    }

    private void RemoveWeaponPassive() {
        Stats.RemoveAllModifiers(m => m.Key.ToString().StartsWith("Weapon"));
    }

    protected void SetDriveDiscsPassive(uint driveDiscSetId, bool partial = false) {
        RemoveDiscSetPassive();
        if (driveDiscSetId == 0) return;

        var set = DriveDiscSetRegistry.CreateInstance(driveDiscSetId);

        if (partial) {
            foreach (var bonus in set.PartialBonus) {
                Stats[bonus.Affix].Add(new(ModifierKey.DiscSet(set.Id), bonus));
            }
        }

        foreach (var bonus in set.FullBonus) {
            Stats[bonus.Affix].Add(new(ModifierKey.DiscSet(set.Id, true), bonus));
        }
    }

    private void RemoveDiscSetPassive() {
        Stats.RemoveAllModifiers(m => m.Key.ToString().StartsWith("Disc"));
    }
}