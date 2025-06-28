using InterknotCalculator.Classes;

namespace InterknotCalculator.Test;

[TestFixture]
public class RotationParseTests {
    [TestCase("1091.aaaaa 1", 0u)]
    [TestCase("aaaaa 1", 1091u)]
    [TestCase("1091.aaaaa", 0u)]
    [TestCase("aaaaa", 1091u)]
    public void ValidRotationTest(string rotation, uint fallbackId) {
        var result = RotationAction.Parse(rotation, fallbackId);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.AgentId, Is.EqualTo(1091));
        Assert.That(result.ActionName, Is.EqualTo("aaaaa"));
        Assert.That(result.Scale, Is.EqualTo(1));
    }
    
    [TestCase("Miyabi.aaaaa 1.5")]
    [TestCase("Miyabi. 12")]
    [TestCase("UnknownAgent.action 1")]
    public void InvalidRotationTest(string rotation) {
        var result = RotationAction.Parse(rotation);
        
        Assert.That(result, Is.Null);
    }
}