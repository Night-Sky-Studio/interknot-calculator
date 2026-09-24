using InterknotCalculator.Core.Enums;

#pragma warning disable CS0618 // Type or member is obsolete

namespace InterknotCalculator.Core.Classes.Agents;

public abstract class RuptureAgent : Agent {
    protected RuptureAgent(uint id) : base(id) {
        Speciality = Speciality.Rupture;
    }
    public Affix RelatedElementSheer => Helpers.GetRelatedSheerDmg(Element);
    
    public double BaseSheerForce => MaxHp * 0.1 + Atk * 0.3;
    public MutableStat SheerForce { get; } = new();
    public MutableStat SheerElementalBonus => Stats[RelatedElementSheer];

    public override SafeDictionary<Affix, double> CollectStats(bool initial = false) {
        var result = base.CollectStats(initial);

        var sheerForce = SheerForce;
        var sheerForceBonus = Stats[Affix.SheerForceBonus];
        var sheerElementalBonus = SheerElementalBonus;
        
        result.Add(Affix.SheerForce, BaseSheerForce + (initial ? sheerForce.InitialValue : sheerForce));
        result.Add(Affix.SheerForceBonus, initial ? sheerForceBonus.InitialValue : sheerForceBonus);
        result.Add(RelatedElementSheer, initial ? sheerElementalBonus.InitialValue : sheerElementalBonus);
        
        return result;
    }
    
    protected override double GetBaseDamage(double scale) => scale / 100 * (BaseSheerForce + SheerForce);
    protected override double GetSheerMultiplier() => Stats[Affix.SheerForceBonus] + SheerElementalBonus;
}