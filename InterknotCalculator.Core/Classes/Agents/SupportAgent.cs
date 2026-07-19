using InterknotCalculator.Core.Classes.DriveDiscSets;
using InterknotCalculator.Core.Classes.Weapons;

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
        
        var weapon = WeaponRegistry.CreateInstance(weaponId);

        if (weapon.Speciality != Speciality) return;
        
        foreach (var passive in weapon.Passive) {
            if (passive.SkillTags.Length != 0) {
                TagBonus.Add(passive);
            } else {
                BonusStats[passive.Affix] += passive.Value;
            }
        }
            
        foreach (var stat in weapon.ExternalBonus) {
            if (stat.SkillTags.Length != 0) {
                ExternalTagBonus.Add(stat);
            } else {
                ExternalBonus[stat.Affix] += stat.Value;
            }
        }
    }

    protected void SetDriveDiscsPassive(uint driveDiscSetId, bool partial = false) {
        if (driveDiscSetId == 0) return;
        
        var set = DriveDiscSetRegistry.CreateInstance(driveDiscSetId);
        
        if (partial) {
            foreach (var bonus in set.PartialBonus) {
                if (bonus.SkillTags.Length != 0) {
                    TagBonus.Add(bonus);
                } else {
                    BonusStats[bonus.Affix] += bonus.Value;
                }
            }
        }
        
        foreach (var bonus in set.FullBonus) {
            if (bonus.SkillTags.Length != 0) {
                TagBonus.Add(bonus);
            } else {
                BonusStats[bonus.Affix] += bonus.Value;
            }
        }
        set.ApplyPassive(this);
    }
}