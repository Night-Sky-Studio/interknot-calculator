namespace InterknotCalculator.Core.Classes.Agents;

public abstract class SupportAgent(uint id) : Agent(id) {
    public void SetWeaponPassive(uint weaponId) {
        var weapon = Resources.Current.GetWeapon(weaponId);

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
    
    public void SetDriveDiscsPassive(uint driveDiscSetId, bool fullBonus = false) {
        var set = Resources.Current.GetDriveDiscSet(driveDiscSetId);
        
        if (!fullBonus) {
            foreach (var bonus in set.PartialBonus) {
                if (bonus.SkillTags.Length != 0) {
                    TagBonus.Add(bonus);
                } else {
                    BonusStats[bonus.Affix] += bonus.Value;
                }
            }
            return;
        } 
        
        foreach (var bonus in set.FullBonus) {
            if (bonus.SkillTags.Length != 0) {
                TagBonus.Add(bonus);
            } else {
                BonusStats[bonus.Affix] += bonus.Value;
            }
        }
        set.ApplyPassive?.Invoke(this);
    }
}