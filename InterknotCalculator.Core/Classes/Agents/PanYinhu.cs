using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class PanYinhu : Agent, IAgentReference<PanYinhu> {
    public static PanYinhu Reference() {
        var panYinhu = new PanYinhu {
            Stats = {
                [Affix.Atk] = 3000
            }
        };

        panYinhu.SetWeapon(WeaponId.TremorTrigramVessel);

        var astralVoid = Resources.Current.GetDriveDiscSet(DriveDiscSetId.AstralVoice);
        
        var qaBonus = astralVoid.FullBonus.First();
        panYinhu.ExternalBonus[qaBonus.Affix] += qaBonus.Value;
        
        panYinhu.ApplyPassive();
        
        return panYinhu;
    }

    public PanYinhu() : base(AgentId.PanYinhu) {
        Speciality = Speciality.Support;
        Element = Element.Physical;
        Rarity = Rarity.A;
        Faction = Faction.YunkuiSummit;
        
        Stats[Affix.Hp] = 8453;
        Stats[Affix.Atk] = 661;
        Stats[Affix.Def] = 712;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 94;
        Stats[Affix.AnomalyMastery] = 91;
        Stats[Affix.AnomalyProficiency] = 90;
        Stats[Affix.EnergyRegen] = 1.56;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.Sheer] = Math.Min((0.18 + 0.06) * InitialAtk, 540 + 180); // M6
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a is {
                Speciality: Speciality.Rupture, Faction: Faction.YunkuiSummit
            })) {
            ExternalBonus[Affix.DmgBonus] = 0.2 + 0.1; // M1
        }
        
        return [];
    }
}