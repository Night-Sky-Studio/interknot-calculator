namespace InterknotCalculator.Core.Classes;

/// <summary>
/// Dictionary class with non-throwing indexer.
/// </summary>
/// <typeparam name="TKey">Dictionary Key (notnull)</typeparam>
/// <typeparam name="TValue">Dictionary Value (struct)</typeparam>
public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull where TValue : struct {
    public new TValue this[TKey key] {
        get => TryGetValue(key, out TValue value) ? value : default;
        set => base[key] = value;
    }
}