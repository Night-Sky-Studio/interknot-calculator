namespace InterknotCalculator.Core.Enums;

public enum Element {
    None,
    Ice, Frost,
    Fire, 
    Physical, HonedEdge,
    Electric, 
    Ether, AuricInk,
    Wind,
    Lumiflux
}

public static class ElementExtensions {
    public static Element GetBaseElement(this Element el) => el switch {
        Element.Ice or Element.Frost => Element.Ice,
        Element.Physical or Element.HonedEdge => Element.Physical,
        Element.Ether or Element.AuricInk => Element.Ether,
        _ => el
    };
    
    /// <summary>
    /// Returns true if the given element belongs to the same element group as the other element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool Matches(this Element element, Element other) {
        return element.GetBaseElement() == other.GetBaseElement();
    }
}