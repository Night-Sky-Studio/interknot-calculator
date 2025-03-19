namespace InterknotCalculator.Classes;

public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull where TValue : struct {
    public new TValue this[TKey key] {
        get => TryGetValue(key, out TValue value) ? value : default;
        set => base[key] = value;
    }
}