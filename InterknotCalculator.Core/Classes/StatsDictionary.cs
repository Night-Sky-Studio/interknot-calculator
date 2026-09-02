using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

/// <summary>
/// Dictionary for agents' stats
/// </summary>
public class StatsDictionary : Dictionary<Affix, MutableStat> {
    public new MutableStat this[Affix key] {
        get {
            if (TryGetValue(key, out var value)) return value;
            
            var stat = new MutableStat();
            Add(key, stat);
            return stat;
        }
        set => base[key] = value;
    }
}