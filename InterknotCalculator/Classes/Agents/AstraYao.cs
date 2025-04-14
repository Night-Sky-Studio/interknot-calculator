﻿using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public sealed class AstraYao : Agent {
    public static AstraYao Reference() {
        var astraYao = new AstraYao {
            Stats = {
                [Affix.Atk] = 3430
            }
        };
        
        astraYao.ApplyPassive();

        var elegantVanity = Resources.Current.GetWeapon(14131);

        foreach (var stat in elegantVanity.ExternalBonus) {
            astraYao.ExternalBonus[stat.Affix] = stat.Value;
        }

        var astralVoice = Resources.Current.GetDriveDiscSet(32800);

        var qaBonus = astralVoice.FullBonus.First();
        foreach (var tag in qaBonus.SkillTags) {
            astraYao.ExternalTagBonus[tag] = qaBonus;
        }

        astraYao.ExternalTagBonus[SkillTag.Chain] = qaBonus with {
            Tags = [SkillTag.Chain]
        };
        
        return astraYao;
    }
    public AstraYao() {
        Speciality = Speciality.Support;
        Element = Element.Ether;
        Rarity = Rarity.S;
        Faction = Faction.StarsOfLyra;
        
        Stats[Affix.Hp] = 7788;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 640 + 75;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 83;
        Stats[Affix.AnomalyMastery] = 93;
        Stats[Affix.AnomalyProficiency] = 92;

        ExternalBonus[Affix.DmgBonus] = 0.2;
        ExternalBonus[Affix.CritDamage] = 0.25;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.Atk] = Math.Min(Stats[Affix.Atk] * 0.35, 1200);
    }
}