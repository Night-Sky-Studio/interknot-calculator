using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public record Anomaly(
    double Scale,
    Element Element,
    bool SelfDisorder = false
) {
    public uint AgentId { get; set; } = 0;
    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    
    /// <summary>
    /// Returns the default anomaly by element
    /// </summary>
    /// <param name="element">Anomaly Element</param>
    /// <returns>Anomaly instance</returns>
    /// <exception cref="ArgumentOutOfRangeException">Anomaly is not found for a given element</exception>
    public static Anomaly GetAnomalyByElement(Element element) => element switch {
        Element.None     => new (0,          Element.None),
        Element.Ice      => new (500.0,      Element.Ice),
        Element.Fire     => new (50.0 * 20,  Element.Fire),
        Element.Physical or Element.HonedEdge 
                         => new (713.0,      Element.Physical),
        Element.Electric => new (125.0 * 10, Element.Electric),
        Element.Ether or Element.AuricInk 
                         => new (62.5 * 20,  Element.Ether),
        _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
    };

    public override string ToString() => Element switch {
        Element.None => "disorder",
        Element.Ice => "shatter",
        Element.Fire => "burn",
        Element.Physical or Element.HonedEdge => "assault",
        Element.Electric => "shock",
        Element.Ether or Element.AuricInk => "corruption",
        Element.Frost => "frostburn",
        _ => throw new ArgumentOutOfRangeException()
    };
}