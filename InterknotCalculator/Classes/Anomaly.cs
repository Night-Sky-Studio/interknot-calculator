using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public enum AnomalyType {
    Assault,
    Burn, 
    Shatter, 
    Shock, 
    Corruption
}

public record Anomaly(double Scale, Element Element, Stat[] Bonuses, bool CanCrit = false) {
    public static readonly Dictionary<AnomalyType, Anomaly> Default = new () {
        { AnomalyType.Assault,    new Anomaly(731.0,      Element.Physical, []) },
        { AnomalyType.Burn,       new Anomaly(50.0 * 20,  Element.Fire,     []) },
        { AnomalyType.Shatter,    new Anomaly(500.0,      Element.Ice,      []) },
        { AnomalyType.Shock,      new Anomaly(125.0 * 10, Element.Electric, []) },
        { AnomalyType.Corruption, new Anomaly(62.5 * 20,  Element.Ether,    []) }
    };

    public static readonly Dictionary<string, Anomaly> DefaultByNames = new () {
        { "assault", Default[AnomalyType.Assault] },
        { "burn", Default[AnomalyType.Burn] },
        { "shatter", Default[AnomalyType.Shatter] },
        { "shock", Default[AnomalyType.Shock] },
        { "corruption", Default[AnomalyType.Corruption] },
    };
    
    public static Anomaly GetAnomalyByElement(Enums.Element element) => element switch {
        Element.Ice => Default[AnomalyType.Shatter],
        Element.Fire => Default[AnomalyType.Burn],
        Element.Physical => Default[AnomalyType.Assault],
        Element.Electric => Default[AnomalyType.Shock],
        Element.Ether => Default[AnomalyType.Corruption]
    };
    
    
}