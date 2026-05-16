namespace InterknotCalculator.Core.Enums;

public enum Element {
    None,
    Ice, Frost,
    Fire, 
    Physical, HonedEdge,
    Electric, 
    Ether, AuricInk
}

public static class ElementExtensions {
    /// <summary>
    /// Returns true if the given element belongs to the same element group as the other element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool Matches(this Element element, Element other) {
        return element switch {
            Element.Ice or Element.Frost 
                => other is Element.Ice or Element.Frost,
            Element.Physical or Element.HonedEdge 
                => other is Element.Physical or Element.HonedEdge,
            Element.Ether or Element.AuricInk 
                => other is Element.Ether or Element.AuricInk,
            _ => false
        };
    }
}