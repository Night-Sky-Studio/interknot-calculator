using System.IO.Pipes;
using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Sunna : SupportAgent, IAgentReference<Sunna>, IEtherVeilAgent<DelusionReprise> {
    public static Sunna Reference(uint weaponId, uint setId) {
        var sunna = new Sunna {
            Stats = {
                [Affix.Atk] = 3500
            }
        };
        
        sunna.SetWeaponPassive(weaponId);
        sunna.SetDriveDiscsPassive(setId);
        
        return sunna;
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

        Skills["mischief_meteor_hammer"] = new(SkillTag.BasicAtk, [
            new(87.4, 38, 12.5, 0.451),
            new(351.9, 164.1, 52.65, 1.896),
            new(362.7, 186.3, 56.74, 2.043),
            new(811.2, 378.2, 116.43, 4.192),
        ]);
        Skills["naughty_cat_spotted"] = new(SkillTag.BasicAtk, [new(417, 0, element: Element.Physical)]);
        Skills["skyward_hammer"] = new(SkillTag.Dash, [new(199.8, 75.2, 19.16, 1.381)]);
        Skills["delusion_strikeout"] = new(SkillTag.Counter, [new(637.8, 413.9, 55.84, 4.022)]);
        Skills["sonic_beatdown"] = new(SkillTag.QuickAssist, [new(146.4, 110.1, 27.92, 2.011)]);
        Skills["stage_fright"] = new(SkillTag.DefensiveAssist, [
            new(0, 481.3),
            new(0, 608.7),
            new(0, 296.2),
        ]);
        Skills["jump_training"] = new(SkillTag.FollowUpAssist, [new(1071.4, 708.2, 193.71)]);
        Skills["star_shooter"] = new(SkillTag.Special, [new(113.6, 85, 21.67)]);
        Skills["bubblegum_barrage"] = new(SkillTag.ExSpecial, [
            new(1827.4, 921.8, 171.02 + 45.81, -70),
            new(1588.3, 742.1, 171.02, -70),
        ]);
        Skills["special_photography_technique"] = new(SkillTag.ExSpecial, [
            new(1656.4, 903.7, 217.16),
            new(1905, 1040.3, 217.16),
        ]);
        Skills["don't_mess_with_the_cat"] = new(SkillTag.Chain, [new(1623.1, 438.9, 212.26)]);
        Skills["smash_it_all"] = new(SkillTag.Ultimate, [new(4182, 559.8, 143.35)]);
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
            // if (ClawSharpenersCount <= 0) { 
            //     CatsGazeActive = false;
            //     return;
            // }
            //
            // ClawSharpenersCount--;
            // CatsGazeActive = true;
            // CatsGazeCooldown = 4;

            var agent = e.Agent;

            if (agent is not { Speciality: Speciality.Attack or Speciality.Anomaly }) return;
                
            var baseDmg = agent.Speciality is Speciality.Anomaly
                ? agent.Atk * 4.8 : agent.Atk * 3.0;
            var critMultiplier = agent.Speciality is Speciality.Anomaly
                ? 1 + 1 * (1.5 + agent.CritDamage)
                : 1 + agent.CritRate * agent.CritDamage;
            var dmgBonus = 1 + agent.ElementalDmgBonus + agent.DmgBonus;
            var resPen = 1 + agent.ElementalResPen + agent.ResPen;
            var stunBonus = 1 + c.Enemy.StunMultiplier;
                
            c.ActionsQueue.Add(new(
                agent.Id,
                "cat's_gaze", 
                SkillTag.DirectHit, 
                baseDmg * critMultiplier * dmgBonus * resPen * c.Enemy.GetDefenseMultiplier(agent.PenRatio, agent.Pen) * stunBonus, 
                0
            ));

            // Reapply "Cat's Gaze" if a "Claw Sharpener" is available,
            // consuming one. Otherwise, let the effect expire
            CatsGazeActive = ClawSharpenersCount > 0;
            if (CatsGazeActive) {
                ClawSharpenersCount--;
                CatsGazeCooldown = 4;
            }
        });
        
        ctx.Events.OnAnomalyTriggered.Add((_, _) => {
            ClawSharpenersCount += 1;
        });
        
        ctx.Events.OnEtherVeilActivated.Add((c, e) => {
            ClawSharpenersCount += 2;

            if (IsTeamPassiveActive && e.Agent == this) {
                c.Enemy.StunMultiplier += 0.3;
            }
        });
        
        ctx.Events.OnEtherVeilDeactivated.Add((c, e) => {
            if (IsTeamPassiveActive && e.Agent == this) {
                c.Enemy.StunMultiplier -= 0.3;
            }
        });
    }
    
    public DelusionReprise EtherVeil { get; } = new();

    private bool IsTeamPassiveActive { get; set; } = false;
    
    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality is Speciality.Attack || a.Faction == Faction)) {
            IsTeamPassiveActive = true;
        }
        
        return [];
    }
}