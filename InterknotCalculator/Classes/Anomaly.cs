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
    public uint AgentId { get; set; }
    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    
    /// <summary>
    /// Default anomalies with default scales
    /// </summary>
    public static readonly Dictionary<AnomalyType, Anomaly> Default = new () {
        { AnomalyType.Disorder,   new (0,          Element.None,     []) },
        { AnomalyType.Assault,    new (731.0,      Element.Physical, []) },
        { AnomalyType.Burn,       new (50.0 * 20,  Element.Fire,     []) },
        { AnomalyType.Shatter,    new (500.0,      Element.Ice,      []) },
        { AnomalyType.Shock,      new (125.0 * 10, Element.Electric, []) },
        { AnomalyType.Corruption, new (62.5 * 20,  Element.Ether,    []) }
    };
    
    /// <summary>
    /// Returns the default anomaly by element
    /// </summary>
    /// <param name="element">Anomaly Element</param>
    /// <returns>Anomaly instance</returns>
    /// <exception cref="ArgumentOutOfRangeException">Anomaly not found for given element</exception>
    public static Anomaly? GetAnomalyByElement(Element element) => element switch {
        Element.None => Default[AnomalyType.Disorder],
        Element.Ice => Default[AnomalyType.Shatter],
        Element.Fire => Default[AnomalyType.Burn],
        Element.Physical => Default[AnomalyType.Assault],
        Element.Electric => Default[AnomalyType.Shock],
        Element.Ether => Default[AnomalyType.Corruption],
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