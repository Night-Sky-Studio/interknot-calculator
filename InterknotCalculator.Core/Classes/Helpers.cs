using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

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
            Element.Ice or Element.Frost => Affix.IceDmgBonus,
            Element.Fire => Affix.FireDmgBonus,
            Element.Electric => Affix.ElectricDmgBonus,
            Element.Physical or Element.HonedEdge => Affix.PhysicalDmgBonus,
            Element.Ether or Element.AuricInk => Affix.EtherDmgBonus,
            _ => Affix.Unknown,
        };
    }
    
    public static Affix GetRelatedSheerDmg(Element? element) {
        return element switch {
            Element.Ice or Element.Frost => Affix.IceSheerBonus,
            Element.Fire => Affix.FireSheerBonus,
            Element.Electric => Affix.ElectricSheerBonus,
            Element.Physical or Element.HonedEdge => Affix.PhysicalSheerBonus,
            Element.Ether or Element.AuricInk => Affix.EtherSheerBonus,
            _ => Affix.Unknown,
        };
    }
    
    /// <summary>
    /// Returns the related ResPen for the given element.
    /// </summary>
    public static Affix GetRelatedAffixRes(Element? element) {
        return element switch {
            Element.Ice or Element.Frost => Affix.IceResPen,
            Element.Fire => Affix.FireResPen,
            Element.Electric => Affix.ElectricResPen,
            Element.Physical or Element.HonedEdge => Affix.PhysicalResPen,
            Element.Ether or Element.AuricInk => Affix.EtherResPen,
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