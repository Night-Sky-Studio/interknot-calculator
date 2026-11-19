using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public static class Helpers {
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
            Element.Wind => Affix.WindDmgBonus,
            _ => Affix.Unknown,
        };
    }
    
    public static Affix GetRelatedSheerDmg(Element? element) {
        return element switch {
            Element.Ice or Element.Frost => Affix.IceSheerDmgBonus,
            Element.Fire => Affix.FireSheerDmgBonus,
            Element.Electric => Affix.ElectricSheerDmgBonus,
            Element.Physical or Element.HonedEdge => Affix.PhysicalSheerDmgBonus,
            Element.Ether or Element.AuricInk => Affix.EtherSheerDmgBonus,
            Element.Wind => Affix.WindSheerDmgBonus,
            _ => Affix.Unknown,
        };
    }

    public static Affix GetRelatedSharpDmg(Element? element) {
        return element switch {
            Element.Ice or Element.Frost => Affix.IceSharpDmgBonus,
            Element.Fire => Affix.FireSharpDmgBonus,
            Element.Electric => Affix.ElectricSharpDmgBonus,
            Element.Physical or Element.HonedEdge => Affix.PhysicalSharpDmgBonus,
            Element.Ether or Element.AuricInk => Affix.EtherSharpDmgBonus,
            Element.Wind => Affix.WindSharpDmgBonus,
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
            Element.Wind => Affix.WindResPen,
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