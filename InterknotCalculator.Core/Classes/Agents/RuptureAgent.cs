using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

public abstract class RuptureAgent : Agent {
    protected RuptureAgent(uint id) : base(id) {
        Speciality = Speciality.Rupture;
    }
    public Affix RelatedSheerDmgElement => Helpers.GetRelatedSheerDmg(Element);
    
    public double BaseSheerForce => MaxHp * 0.1 + Atk * 0.3;
    public MutableStat SheerForce { get; } = new();
    public MutableStat ElementalSheerDmgBonus => GetStat(RelatedSheerDmgElement);

    public override SafeDictionary<Affix, double> CollectStats(bool initial = false) {
        var result = base.CollectStats(initial);

        var sheerForce = SheerForce;
        var sheerForceBonus = GetStat(Affix.SheerDmgBonus);
        var sheerElementalBonus = ElementalSheerDmgBonus;
        
        result.Add(Affix.SheerForce, BaseSheerForce + (initial ? sheerForce.InitialValue : sheerForce));
        result.Add(Affix.SheerDmgBonus, initial ? sheerForceBonus.InitialValue : sheerForceBonus);
        result.Add(RelatedSheerDmgElement, initial ? sheerElementalBonus.InitialValue : sheerElementalBonus);
        
        return result;
    }
    
    protected override double GetBaseDamage(in Hit hit) => hit.MotionValue.Damage / 100 * (BaseSheerForce + SheerForce);
    protected override double GetEnemyDefenseMultiplier(in Hit hit) => 1;
    protected override double GetSpecialityMultiplier(in Hit hit) => 
        1 + GetStat(Helpers.GetRelatedSheerDmg(hit.Element)).For(hit.Ability) 
          + GetStat(Affix.SheerDmgBonus).For(hit.Ability);
}