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

    /// <summary>Detaches every modifier with <paramref name="key"/> from every stat.</summary>
    public void RemoveAllModifiers(ModifierKey key) {
        foreach (var stat in Values) {
            stat.RemoveKey(key);
        }
    }

    /// <summary>Detaches every modifier matching <paramref name="predicate"/> from every stat.</summary>
    public void RemoveAllModifiers(Func<Modifier, bool> predicate) {
        foreach (var stat in Values) {
            foreach (var modifier in stat.Modifiers.Where(predicate).ToArray()) {
                stat.Remove(modifier);
            }
        }
    }
}