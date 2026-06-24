using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public sealed class AstraYao : SupportAgent, IAgentReference<AstraYao> {
    public static AstraYao Reference(uint weaponId, uint setId) {
        var astraYao = new AstraYao {
            Stats = {
                [Affix.Atk] = 3430
            }
        };

        astraYao.SetWeaponPassive(weaponId);
        astraYao.SetDriveDiscsPassive(setId);
        
        astraYao.ApplyPassive();
        
        return astraYao;
    }
    public AstraYao() : base(AgentId.AstraYao) {
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
        Stats[Affix.EnergyRegen] = 1.56;

        ExternalBonus[Affix.DmgBonus] = 0.2;
        ExternalBonus[Affix.CritDamage] = 0.25;
    }

    public override void ApplyPassive() {
        ExternalBonus[Affix.Atk] += Math.Min(InitialAtk * 0.35, 1200);
    }
}