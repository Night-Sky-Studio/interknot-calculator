using InterknotCalculator.Core.Classes.Events;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Koleda : SupportAgent, IAgentReference<Koleda> {
    public static Koleda Reference(uint weaponId, uint setId) {
        var koleda = new Koleda();

        koleda.InitializeStats(new() {
            [Affix.CritRate] = 0.5
        });
        // TODO: Needs abilities to track this
        // Will be permanently activated for in Reference
        koleda.IsFurnaceFireActive = true;

        koleda.SetWeaponPassive(weaponId);
        koleda.SetDriveDiscsPassive(setId);

        return koleda;
    }

    public Koleda() : base(AgentId.Koleda) {
        Speciality = Speciality.Stun;
        Element = Element.Fire;
        Rarity = Rarity.S;
        Faction = Faction.BelobogHeavyIndustries;

        InitializeStats(new() {
            [Affix.Hp] = 8127,
            [Affix.Atk] = 660 + 75,
            [Affix.Def] = 594,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 116 + 18,
            [Affix.AnomalyMastery] = 97,
            [Affix.AnomalyProficiency] = 96,
            [Affix.EnergyRegen] = 1.2
        });
    }

    /// <summary>
    /// Furnace Fire state. The setter keeps the daze modifier in sync, so the modifier
    /// exists exactly while the effect is active.
    /// </summary>
    private bool IsFurnaceFireActive {
        get;
        set {
            field = value;
            DazeBonus.RemoveKey(ModifierKey.Agent(Id) + ModifierKey.CorePassive() + ModifierKey.AbilityPassive());
            if (value) {
                DazeBonus.AddUnique(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive() + ModifierKey.AbilityPassive(),
                    0.6, ModifierType.CombatFlat,
                    tags: SkillTag.BasicAtk));
            }
        }
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);

        ctx.Events.OnCalculationStarted += c => {
            DazeBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                0.6, ModifierType.CombatFlat,
                tags: SkillTag.ExSpecial));
            
            // When Furnace Fire is consumed, all Agents deal 35% more damage for 40s.
            foreach (var agent in c.Team.Values) {
                agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 
                    0.35, ModifierType.CombatFlat));
            }

            if (c.HasTeammates(a => a.Element.Matches(Element)
                                    || a.Faction == Faction
                                    || a.Speciality is Speciality.Rupture, Id)
                && c.Enemy.IsStunned) {
                foreach (var agent in c.Team.Values) {
                    agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 
                        0.35, ModifierType.CombatFlat,
                        tags: SkillTag.Chain));
                }
            }
        };

        // ctx.Events.OnActionExecuting += OnActionExecuting;
        // ctx.Events.OnActionExecuted += OnActionExecuted;
    }
    
    // Granting happens before damage, so it could affect the granting ability itself
    private void OnActionExecuting(Context _, ActionExecutingEventArgs e) {
        if (e.Agent != this || IsFurnaceFireActive) return;
        if (e.Ability is { Tag: SkillTag.FollowUpAssist, Name: "hammer_bell" }
            or { Tag: SkillTag.ExSpecial, Name: "boiling_furnace" }
            or { Tag: SkillTag.Chain, Name: "natural_disaster" }
            or { Tag: SkillTag.Ultimate, Name: "hammerquake" }) {
            IsFurnaceFireActive = true;
        }
    }
    
    // Consumption happens after damage, so the consuming hit still gets the bonus
    private void OnActionExecuted(Context _, ActionExecutedEventArgs e) {
        if (e.Agent != this || !IsFurnaceFireActive) return;
        if (e.Ability is { Tag: SkillTag.BasicAtk, Name: "smash_n_bash", Scale: 2 }) {
            IsFurnaceFireActive = false;
        }
    }
}