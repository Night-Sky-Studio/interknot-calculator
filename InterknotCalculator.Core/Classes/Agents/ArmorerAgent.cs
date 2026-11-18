using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

/// <summary>
/// Base Armorer Agent class
/// </summary>
/// <remarks>
/// Armorer agents deal Sharp DMG:
/// <c>Base(DEF) * DMG% * Laceration * RES * DEF * Stun * DMG Taken * Sharp DMG%</c>
/// Their hits build up attribute anomaly as usual, and also build up Gash on the enemy
/// </remarks>
public abstract class ArmorerAgent : Agent {
    protected ArmorerAgent(uint id) : base(id) {
        Speciality = Speciality.Armorer;
    }
    
    protected abstract Skill Maim { get; }

    public Affix RelatedSharpDmgElement => Helpers.GetRelatedSharpDmg(Element);
    
    public MutableStat LacerationDmg => GetStat(Affix.LacerationDmg);
    public MutableStat SharpDmgBonus => GetStat(Affix.SharpDmgBonus);
    public MutableStat ElementalSharpDmgBonus => GetStat(RelatedSharpDmgElement);
    public MutableStat GashBuildupBonus => GetStat(Affix.GashBuildupBonus);

    public override SafeDictionary<Affix, double> CollectStats(bool initial = false) {
        var result = base.CollectStats(initial);

        Add(Affix.LacerationDmg, LacerationDmg);
        Add(Affix.SharpDmgBonus, SharpDmgBonus);
        Add(RelatedSharpDmgElement, ElementalSharpDmgBonus);
        Add(Affix.GashBuildupBonus, GashBuildupBonus);

        return result;

        void Add(Affix affix, MutableStat stat) {
            var value = initial ? stat.InitialValue : stat.Value;
            if (value != 0) result[affix] = value;
        }
    }

    protected override double GetBaseDamage(in Hit hit) => hit.MotionValue.Damage / 100 * Def;

    protected override double GetCritMultiplier(in Hit hit) =>
        GetLacerationMultiplier(CritRate.For(hit.Ability), LacerationDmg.For(hit.Ability));

    /// <summary>
    /// Expected Laceration modifier
    /// </summary>
    /// <remarks>
    /// Sharp DMG ignores CRIT DMG <br/>
    /// The Critical check succeeds with chance <c>min(CR, 1)</c>. If it succeeds, the Laceration
    /// check succeeds with chance <c>clamp(CR - 1, 0, 1)</c> and each success multiplies by <c>1 + L</c>
    /// </remarks>
    protected static double GetLacerationMultiplier(double critRate, double laceration) {
        var critical = Math.Min(critRate, 1);
        var secondCheck = Math.Clamp(critRate - 1, 0, 1);
        
        return 1 + critical * laceration + secondCheck * laceration * (1 + laceration);
    }

    protected override double GetSpecialityMultiplier(in Hit hit) =>
        1 + GetStat(Helpers.GetRelatedSharpDmg(hit.Element)).For(hit.Ability)
          + SharpDmgBonus.For(hit.Ability);

    public double GetGashBuildup(Ability ability) {
        var baseBuildup = GetSkill(ability).Scales[ability.Scale].GashBuildup;
        if (baseBuildup == 0) return 0;

        return baseBuildup * (1 + GashBuildupBonus.For(ability));
    }

    public override AgentAction GetActionDamage(Context ctx, Ability ability) {
        // Attribute anomaly buildup is added by the base formula
        var action = base.GetActionDamage(ctx, ability);
        ctx.Enemy.AddGashBuildup(ctx, this, GetGashBuildup(ability));
        return action;
    }

    protected override Skill GetSkill(in Ability ability) => 
        ability.Name == "maim" ? Maim : base.GetSkill(ability);

    /// <summary>
    /// Deals the damage of the agent's "maim" skill and raises <see cref="Events.EventBus.OnMaim"/>.
    /// The caller consumes the Gash stack.
    /// </summary>
    protected AgentAction TriggerMaim(Context ctx) {
        var action = GetActionDamage(ctx, new(Maim.Tag, "maim"));
        ctx.Events.Maim(ctx, new(this));
        return action;
    }
}
