using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

public abstract class RuptureAgent : Agent {
    protected RuptureAgent(uint id) : base(id) {
        Speciality = Speciality.Rupture;
    }
    public Affix RelatedElementSheer => Helpers.GetRelatedSheerDmg(Element);
    
    public double SheerForce => MaxHp * 0.1 + Atk * 0.3;
    public double SheerElementalBonus => Stats[RelatedElementSheer] + BonusStats[RelatedElementSheer];

    public override SafeDictionary<Affix, double> CollectStats() {
        var result = base.CollectStats();
        result.Add(Affix.Sheer, SheerForce);
        result.Add(Affix.SheerBonus, BonusStats[Affix.SheerBonus]);
        result.Add(RelatedElementSheer, SheerElementalBonus);
        return result;
    }
    
    protected override double GetBaseDamage(double scale) => scale / 100 * SheerForce;
    protected override double GetSheerMultiplier() => BonusStats[Affix.SheerBonus] + SheerElementalBonus;
}