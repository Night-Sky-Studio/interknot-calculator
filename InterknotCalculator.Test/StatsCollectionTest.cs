using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test;

[TestFixture]
public class StatsCollectionTest: CalculatorTest {
    private void CheckStats(Dictionary<Affix, double> expected, Dictionary<Affix, double> actual) {
        foreach (var (key, value) in expected) {
            Console.WriteLine($"Testing {key}");
            var actualValue = actual[key] > 2 ? Math.Floor(actual[key]) : actual[key];
            Assert.That(actualValue, Is.EqualTo(value).Within(0.001), $"Key: {key}");
        }
    }
    
    [Test]
    public void MiyabiStatsTest() {
        var reference = new SafeDictionary<Affix, double> {
            [Affix.Hp] = 11_129,
            [Affix.Atk] = 2_899,
            [Affix.Def] = 790,
            [Affix.Impact] = 86,
            [Affix.CritRate] = 0.706,
            [Affix.CritDamage] = 1.668,
            [Affix.EnergyRegen] = 1.2,
            [Affix.AnomalyProficiency] = 247,
            [Affix.AnomalyMastery] = 116,
            [Affix.Pen] = 36,
            [Affix.PenRatio] = 0.24
        };
        
        var miyabi = new Miyabi();
        miyabi.SetWeapon(WeaponId.HailstormShrine);
        miyabi.SetDriveDiscs([
            new(DriveDiscSetId.WoodpeckerElectro, 1, Rarity.S,
                Stat.Stats[Affix.Hp],
                [
                    Stat.SubStats[Affix.HpRatio] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage],
                    Stat.SubStats[Affix.CritRate] with { Level = 3 },
                    Stat.SubStats[Affix.AtkRatio] with { Level = 3 }
                ]),
            new(DriveDiscSetId.BranchBladeSong, 2, Rarity.S,
                Stat.Stats[Affix.Atk],
                [
                    Stat.SubStats[Affix.AnomalyProficiency],
                    Stat.SubStats[Affix.CritDamage] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate] with { Level = 3 },
                    Stat.SubStats[Affix.Pen]
                ]),
            new(DriveDiscSetId.WoodpeckerElectro, 3, Rarity.S,
                Stat.Stats[Affix.Def],
                [
                    Stat.SubStats[Affix.AtkRatio] with { Level = 2 },
                    Stat.SubStats[Affix.CritRate] with { Level = 4 },
                    Stat.SubStats[Affix.CritDamage],
                    Stat.SubStats[Affix.Atk] with { Level = 2 }
                ]),
            new(DriveDiscSetId.BranchBladeSong, 4, Rarity.S,
                Stat.Stats[Affix.CritDamage],
                [
                    Stat.SubStats[Affix.AtkRatio],
                    Stat.SubStats[Affix.Pen] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate] with { Level = 3 },
                    Stat.SubStats[Affix.Atk] with { Level = 2 }
                ]),
            new(DriveDiscSetId.BranchBladeSong, 5, Rarity.S,
                Stat.Stats[Affix.PenRatio],
                [
                    Stat.SubStats[Affix.HpRatio] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage] with { Level = 4 },
                    Stat.SubStats[Affix.AtkRatio],
                    Stat.SubStats[Affix.Hp]
                ]),
            new(DriveDiscSetId.BranchBladeSong, 6, Rarity.S,
                Stat.Stats[Affix.AtkRatio],
                [
                    Stat.SubStats[Affix.Hp] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage] with { Level = 2 },
                    Stat.SubStats[Affix.Atk] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate]
                ])
        ]);
        
        CheckStats(reference, miyabi.BaseStats);
        //Assert.That(miyabi.BaseStats, Is.EquivalentTo(reference));
    }
}