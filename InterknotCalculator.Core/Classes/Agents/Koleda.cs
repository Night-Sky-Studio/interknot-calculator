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

    private bool IsFurnaceFireActive { get; set; }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);

        ctx.Events.OnCalculationStarted.Add(c => {
            DazeBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.6,
                tags: SkillTag.ExSpecial));

            if (c.Team.Values.Any(a => a.Element.Matches(Element)
                                       || a.Faction == Faction
                                       || a.Speciality is Speciality.Rupture)
                && c.Enemy.StunMultiplier > 1.0) {
                foreach (var agent in c.Team.Values) {
                    agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.35 * 2,
                        tags: SkillTag.Chain));
                }
            }
        });

        // ctx.Events.OnActionExecuted.Add(OnActionExecuted);
    }

    private void OnActionExecuted(Context ctx, ActionExecutedEventArgs e) {
        IsFurnaceFireActive = e.Ability switch {
            { Tag: SkillTag.FollowUpAssist, Name: "hammer_bell" }
                or { Tag: SkillTag.ExSpecial, Name: "boiling_furnace" }
                or { Tag: SkillTag.Chain, Name: "natural_disaster" }
                or { Tag: SkillTag.Ultimate, Name: "hammerquake" }
                => true,
            { Tag: SkillTag.BasicAtk, Name: "smash_n_bash", Scale: 2 }
                => false,
            _ => IsFurnaceFireActive
        };
    }

    public override Stat? ApplyAbilityPassive(Ability ability) {
        return ability.Tag switch {
            SkillTag.ExSpecial => new(Affix.DazeBonus, 0.6),
            SkillTag.BasicAtk => IsFurnaceFireActive
                ? new(Affix.DazeBonus, 0.6)
                : null,
            _ => null
        };
    }
}