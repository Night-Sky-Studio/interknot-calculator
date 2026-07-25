using System.IO.Pipes;
using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Sunna : SupportAgent, IAgentReference<Sunna>, IEtherVeilAgent<DelusionReprise> {
    public static Sunna Reference(uint weaponId, uint setId) {
        return new();
    }

    private int ClawSharpenersCount {
        get;
        set => field = Math.Clamp(value, 0, 6);
    } = 0;
    private bool CatsGazeActive { get; set; } = false;
    private int CatsGazeCooldown { get; set; } = 4;
    
    public Sunna() : base(AgentId.Sunna) {
        Speciality = Speciality.Support;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.AngelsOfDelusion;
            
        Stats[Affix.Hp] = 8477;
        Stats[Affix.Def] = 600;
        Stats[Affix.Atk] = 750;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 98;
        Stats[Affix.AnomalyMastery] = 96;
        Stats[Affix.AnomalyProficiency] = 95;
        Stats[Affix.EnergyRegen] = 1;
    }

    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnActionExecuted.Add((c, e) => {
            if (e.Ability is { Tag: Enums.SkillTag.ExSpecial, Name: "special_photography_technique" }) {
                c.ReactivateEtherVeil(this, EtherVeil);
            }
        });
        
        ctx.Events.OnActionExecuted.Add((c, e) => {
            if (e.Agent == this && e.Ability.Name is "naughty_cat_spotted"
                    or "bubblegum_barrage"
                    or "special_photography_technique"
                    or "don't_mess_with_the_cat"
                    or "smash_it_all") {
                CatsGazeActive = true;
            } else {
                if (c.Enemy.StunMultiplier > 1 && CatsGazeActive) {
                    CatsGazeCooldown -= 4;
                } else {
                    CatsGazeCooldown--;
                }
            }
        });

        ctx.Events.OnActionExecuted.Add((c, e) => {
            if (!CatsGazeActive || CatsGazeCooldown != 0) return;
            CatsGazeActive = false;

            var agent = e.Agent;

            if (agent is not { Speciality: Speciality.Attack or Speciality.Anomaly }) return;
                
            var baseDmg = agent.Speciality is Speciality.Anomaly
                ? agent.Atk * 4.8 : agent.Atk * 3.0;
            var critMultiplier = agent.Speciality is Speciality.Anomaly
                ? 1 + 1 * (1.5 + agent.CritDamage)
                : 1 + agent.CritRate * agent.CritDamage;
            var dmgBonus = 1 + agent.ElementalDmgBonus + agent.DmgBonus;
            var resPen = 1 + agent.ElementalResPen + agent.ResPen;
                
            c.ActionsQueue.Add(new(
                c.MainAgentId,
                "cat's_gaze", 
                SkillTag.DirectHit, 
                baseDmg * critMultiplier * dmgBonus * resPen * c.Enemy.GetDefenseMultiplier(agent.PenRatio, agent.Pen), 
                0
            ));

            if (ClawSharpenersCount <= 0) return;
            
            ClawSharpenersCount--;
            CatsGazeActive = true;
        });
        
        ctx.Events.OnAnomalyTriggered.Add((_, _) => {
            ClawSharpenersCount += 1;
        });
        
        ctx.Events.OnEtherVeilActivated.Add((_, _) => {
            ClawSharpenersCount += 2;
        });
    }


    public DelusionReprise EtherVeil { get; } = new();
}