namespace InterknotCalculator.Classes;

public struct Progress {
    public double Value { get; set; }
    public double Maximum { get; set; }
    
    public static implicit operator double(Progress progress) {
        return progress.Value;
    }
}