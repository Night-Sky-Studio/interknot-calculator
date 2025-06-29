using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public enum AnomalyType {
    Disorder,
    Assault,
    Burn, 
    Shatter, 
    Shock, 
    Corruption
}

public record Anomaly(
    double Scale,
    Element Element,
    Stat[] Bonuses,
    bool CanCrit = false
) {
    public uint AgentId { get; set; } = 0;
    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    
    /// <summary>
    /// Returns the default anomaly by element
    /// </summary>
    /// <param name="element">Anomaly Element</param>
    /// <returns>Anomaly instance</returns>
    /// <exception cref="ArgumentOutOfRangeException">Anomaly not found for given element</exception>
    public static Anomaly? GetAnomalyByElement(Element element) => element switch {
        Element.None     => new (0,          Element.None,     []),
        Element.Ice      => new (500.0,      Element.Ice,      []),
        Element.Fire     => new (50.0 * 20,  Element.Fire,     []),
        Element.Physical => new (731.0,      Element.Physical, []),
        Element.Electric => new (125.0 * 10, Element.Electric, []),
        Element.Ether or Element.AuricInk => new (62.5 * 20,  Element.Ether,    []),
        _ => null
    };

    public override string ToString() => Element switch {
        Element.None => "disorder",
        Element.Ice => "shatter",
        Element.Fire => "burn",
        Element.Physical => "assault",
        Element.Electric => "shock",
        Element.Ether => "corruption",
        Element.Frost => "frostburn",
        Element.AuricInk => "corruption",
        _ => throw new ArgumentOutOfRangeException()
    };
}