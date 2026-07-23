using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Zhao : SupportAgent, IAgentReference<Zhao>, IEtherVeilAgent<Wellspring> {
    public static Zhao Reference(uint weaponId, uint setId) {
        var zhao = new Zhao();

        zhao.SetWeaponPassive(weaponId);
        zhao.SetDriveDiscsPassive(setId);

        return zhao;
    }

    public Zhao() : base(AgentId.Zhao) {
        Speciality = Speciality.Defense;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.KrampusComplianceAuthority;

        Stats[Affix.Hp] = 9117;
        Stats[Affix.Def] = 301;
        Stats[Affix.Atk] = 765;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 93;
        Stats[Affix.AnomalyMastery] = 93;
        Stats[Affix.AnomalyProficiency] = 96;

        Skills["burst_of_frost"] = new(SkillTag.Entry, [
            new(1361.2, 345.7, 209.18, 7.531)
        ]);
    }

    private bool IsTeamPassiveActive { get; set; } = false;
    private bool IsTeamPassiveActivated { get; set; } = false;
    
    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnActionExecuted.Add((c, e) => {
            // Zhao triggered an action
            if (e.Agent != this) return;

            // Zhao used an entry or ex-special
            if (e.Ability is not { Tag: SkillTag.Entry or SkillTag.ExSpecial }) return;

            // Assume always full Frostbite Points, because I can't be bothered
            if (c.GetEtherVeil<Wellspring>() is { } wellspring) {
                // Overwrite existing Ether Veil if it already exists
                c.DeactivateEtherVeil(this, wellspring);
            }
            c.ActivateEtherVeil(this, EtherVeil);
        });

        // Core Passive
        ctx.Events.OnEtherVeilActivated.Add((c, e) => {
            // Team passive
            if (IsTeamPassiveActive && !IsTeamPassiveActivated) {
                IsTeamPassiveActivated = true;
                foreach (var (_, agent) in c.Team) {
                    agent.BonusStats[Affix.DmgBonus] += 0.1 + Math.Min(0.4, Math.Max(0, MaxHp - 15000) / 400);
                }
            }
            
            if (e.Agent != this && e.EtherVeil is not Wellspring) return;

            foreach (var (_, agent) in c.Team) {
                agent.BonusStats[Affix.Atk] += 1000;
            }
        });
        ctx.Events.OnEtherVeilDeactivated.Add((c, e) => {
            if (e.Agent != this && e.EtherVeil is not Wellspring) return;

            foreach (var (_, agent) in c.Team) {
                agent.BonusStats[Affix.Atk] -= 1000;
            }
        });
    }
    
    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality is Speciality.Attack or Speciality.Anomaly or Speciality.Support)) {
            IsTeamPassiveActive = true;
        }

        return [];
    }

    public Wellspring EtherVeil { get; } = new();
}