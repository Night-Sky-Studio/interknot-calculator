using InterknotCalculator.Classes;

namespace InterknotCalculator.Test;

[TestFixture]
public class CalculatorTest {
    [OneTimeSetUp]
    public async Task OneTimeSetUp() {
        await Resources.Current.Init();
    } 
    
    protected Calculator Calculator { get; } = new();
}