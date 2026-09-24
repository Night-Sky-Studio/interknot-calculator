using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Zhao : SupportAgent, IAgentReference<Zhao>, IEtherVeilAgent<Wellspring> {
    public static Zhao Reference(uint weaponId, uint setId) {
        var zhao = new Zhao();
        
        zhao.InitializeStats(new () {
            [Affix.Hp] = 27000
        });

        zhao.SetWeaponPassive(weaponId);
        zhao.SetDriveDiscsPassive(setId);

        return zhao;
    }

    public Zhao() : base(AgentId.Zhao) {
        Speciality = Speciality.Defense;
        Element = Element.Ice;
        Rarity = Rarity.S;
        Faction = Faction.KrampusComplianceAuthority;

        InitializeStats(new () {
            [Affix.Hp] = 9117,
            [Affix.Def] = 301,
            [Affix.Atk] = 765,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 93,
            [Affix.AnomalyMastery] = 93,
            [Affix.AnomalyProficiency] = 96
        });

        Skills["burst_of_frost"] = new(SkillTag.Entry, [
            new(1361.2, 345.7, 209.18, 7.531)
        ]);
    }

    private bool IsTeamPassiveActive { get; set; }

    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnCalculationStarted.Add(c => {
            if (c.Team.Values.Any(a => a.Speciality is Speciality.Attack or Speciality.Anomaly or Speciality.Support)) {
                IsTeamPassiveActive = true;
            }
        });
        
        ctx.Events.OnActionExecuted.Add((c, e) => {
            // Zhao triggered an action
            if (e.Agent != this) return;

            // Zhao used an entry or ex-special
            if (e.Ability is not { Tag: SkillTag.Entry or SkillTag.ExSpecial }) return;

            // Assume always full Frostbite Points, because I can't be bothered.
            // Overwrites the existing Ether Veil if it already exists
            c.ReactivateEtherVeil(this, EtherVeil);
        });

        // Core Passive
        ctx.Events.OnEtherVeilActivated.Add((c, e) => {
            // Team passive
            if (IsTeamPassiveActive) {
                foreach (var agent in c.Team.Values) {
                    agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 
                        0.1 + Math.Min(0.4, Math.Max(0, MaxHp - 15000) / 400)));
                }
            }
            
            if (e.Agent != this || e.EtherVeil is not Wellspring) return;

            foreach (var agent in c.Team.Values) {
                agent.Atk.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 1000, ModifierType.CombatFlat));
            }
        });
        ctx.Events.OnEtherVeilDeactivated.Add((c, e) => {
            if (e.Agent != this || e.EtherVeil is not Wellspring) return;

            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.RemoveKey(ModifierKey.Agent(Id) + ModifierKey.TeamPassive());
                agent.Atk.RemoveKey(ModifierKey.Agent(Id) + ModifierKey.CorePassive());
            }
        });
    }

    public Wellspring EtherVeil { get; } = new();
}