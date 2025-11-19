using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class JuFufu : Agent, IStunAgent, IAgentReference<JuFufu> {
    public static JuFufu Reference() {
        var fufu = new JuFufu {
            Stats = {
                [Affix.CritRate] = 0.5
            }
        };
        fufu.SetWeapon(WeaponId.RoaringFurnace);

        var kots = Resources.Current.GetDriveDiscSet(DriveDiscSetId.KingOfTheSummit);
        foreach (var fullBonus in kots.FullBonus) {
            fufu.ExternalBonus[fullBonus.Affix] += fullBonus.Value;
        }
        
        fufu.ApplyPassive();
        return fufu;
    }

    public double EnemyStunBonusOverride { get; set; } = 0; // :(

    public JuFufu() : base(AgentId.JuFufu) {
        Speciality = Speciality.Stun;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.YunkuiSummit;
        
        Stats[Affix.Hp] = 8250;
        Stats[Affix.Def] = 765;
        Stats[Affix.Atk] = 597;
        Stats[Affix.CritRate] = 0.194;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 118;
        Stats[Affix.AnomalyMastery] = 93;
        Stats[Affix.AnomalyProficiency] = 96;
        Stats[Affix.EnergyRegen] = 1.2;
    }

    public override void ApplyPassive() {
        // every 100 atk over 2800, crit dmg + 0.05
        var atkBase = Math.Max(InitialAtk - 2800, 0);
        ExternalBonus[Affix.CritDamage] += 0.2 + Math.Min(atkBase / 5, 0.3);
        ExternalTagBonus.Add(new (Affix.DmgBonus, 0.2, tags: [SkillTag.Chain]));
        ExternalTagBonus.Add(new (Affix.DmgBonus, 0.4, tags: [SkillTag.Ultimate]));
        BonusStats[Affix.Impact] += 50;
    }
    
}