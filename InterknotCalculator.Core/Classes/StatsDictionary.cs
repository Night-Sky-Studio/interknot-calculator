using System.Collections.Immutable;
using InterknotCalculator.Core.Classes.Modifiers;
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

    public void RemoveAllModifiers(ModifierKey key) {
        foreach (var stat in Values) {
            stat.RemoveKey(key);
        }
    }

    public void RemoveAllModifiers(Func<Modifier, bool> predicate) {
        foreach (var stat in Values) {
            var toRemove = stat.AppliedModifiers
                .Where(predicate)
                .Select(m => m.Key)
                .ToImmutableArray();
            foreach (var key in toRemove) {
                stat.RemoveKey(key);
            }
        }
    }
}