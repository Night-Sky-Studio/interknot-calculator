using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Server;

public record FinalStats {
    public Dictionary<Affix, double> BaseStats { get; set; } = new();
    public Dictionary<Affix, double> CalculatedStats { get; set; } = new();
}