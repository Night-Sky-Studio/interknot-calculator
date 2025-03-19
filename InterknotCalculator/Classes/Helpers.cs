using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public class Helpers {
    public static Element? GetRelatedElement(Affix affix) {
        return affix switch {
            Affix.IceDmgBonus => Element.Ice,
            Affix.IceResPen => Element.Ice,
            Affix.FireDmgBonus => Element.Fire,
            Affix.FireResPen => Element.Fire,
            Affix.ElectricDmgBonus => Element.Electric,
            Affix.ElectricResPen => Element.Electric,
            Affix.PhysicalDmgBonus => Element.Physical,
            Affix.PhysicalResPen => Element.Physical,
            Affix.EtherDmgBonus => Element.Ether,
            Affix.EtherResPen => Element.Ether,
            _ => null,
        };
    }

    public static Affix GetRelatedAffixDmg(Element? element) {
        return element switch {
            Element.Ice => Affix.IceDmgBonus,
            Element.Fire => Affix.FireDmgBonus,
            Element.Electric => Affix.ElectricDmgBonus,
            Element.Physical => Affix.PhysicalDmgBonus,
            Element.Ether => Affix.EtherDmgBonus,
            _ => Affix.Unknown,
        };
    }
    
    public static Affix GetRelatedAffixRes(Element? element) {
        return element switch {
            Element.Ice => Affix.IceResPen,
            Element.Fire => Affix.FireResPen,
            Element.Electric => Affix.ElectricResPen,
            Element.Physical => Affix.PhysicalResPen,
            Element.Ether => Affix.EtherResPen,
            _ => Affix.Unknown,
        };
    }
}