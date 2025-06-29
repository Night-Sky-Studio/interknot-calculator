using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public static class Helpers {
    
    /// <summary>
    /// Returns the related element for the given DmgBonus or ResPen.
    /// </summary>
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

    /// <summary>
    /// Returns the related DmgBonus for the given element.
    /// </summary>
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
    
    /// <summary>
    /// Returns the related ResPen for the given element.
    /// </summary>
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

    #if DEBUG
    public static void BreakIf(Func<bool> condition) {
        if (System.Diagnostics.Debugger.IsAttached && condition()) {
            System.Diagnostics.Debugger.Break();
        }
    }
    #endif
}