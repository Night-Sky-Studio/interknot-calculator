using System.Text.Json;
using InterknotCalculator.Core.Classes.Modifiers;

namespace InterknotCalculator.Test;

[TestFixture]
public class ModifierKeyTests {
    [Test]
    public void BaseFunctionalityTest() {
        var simpleKey = new ModifierKey("Passive");
        var complexKey = new ModifierKey("Agent:1401", "Weapon:14140");
        var combinedKey = complexKey.CombineWith(simpleKey);

        using (Assert.EnterMultipleScope()) {
            Assert.That(simpleKey.ToString(), Is.EqualTo("Passive"));
            Assert.That(complexKey.ToString(), Is.EqualTo("Agent:1401;Weapon:14140"));
            Assert.That(combinedKey.ToString(), Is.EqualTo("Agent:1401;Weapon:14140;Passive"));
        }
    }

    [Test]
    public void OperatorsTest() {
        var left = new ModifierKey("Agent:1401");
        var right = new ModifierKey("Disc:1");

        using (Assert.EnterMultipleScope()) {
            Assert.That(left + right, Is.EqualTo(left.CombineWith(right)));
            Assert.That(left, Is.Not.EqualTo(right));
        }
    }
    
    [Test]
    public void ComponentsMatchingTest() {
        var key1 = new ModifierKey("Weapon:14140", "Passive");
        Assert.That(key1.ComponentStartsWith("Weapon:"), Is.True);
        
        var key2 = new ModifierKey("Agent:1401", "Disc:Set:32600:full");
        using (Assert.EnterMultipleScope()) {
            Assert.That(key2.ComponentStartsWith("Disc:"), Is.True);
            Assert.That(key2.ComponentStartsWith("Agent:"), Is.True);
        }
        
        var key3 = new ModifierKey("Agent:1401", "Weapon:14140");
        Assert.Multiple(() => {
            Assert.That(key3.ComponentStartsWith("Disc:"), Is.False);
            Assert.That(key3.ComponentStartsWith("1401"), Is.False);
        });
    }

    [Test]
    public void EqualityContractTest() {
        var a = new ModifierKey("Agent:1401", "Disc:1");
        var b = new ModifierKey("Agent:1401", "Disc:1");
        var different = new ModifierKey("Agent:1401", "Disc:2");

        using (Assert.EnterMultipleScope()) {
            Assert.That(a, Is.EqualTo(b));
            Assert.That(a, Is.EqualTo(b));
            Assert.That(a, Is.Not.EqualTo(different));
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
            Assert.That(a, Is.EqualTo(b));
            Assert.That(a.ToString(), Is.EqualTo("Agent:1401;Disc:1"));
        }
    }
    

    [Test]
    public void JsonConverterTest() {
        var key = new ModifierKey("Agent:1401", "Disc:Set:32600:full");

        var json = JsonSerializer.Serialize(key);
        var restored = JsonSerializer.Deserialize<ModifierKey>(json);

        using (Assert.EnterMultipleScope()) {
            Assert.That(json, Is.EqualTo("\"Agent:1401;Disc:Set:32600:full\""));
            Assert.That(restored, Is.EqualTo(key));
            Assert.That(restored.ComponentStartsWith("Disc:"), Is.True);
        }
    }
}