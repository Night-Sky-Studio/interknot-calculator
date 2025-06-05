using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Server;

namespace InterknotCalculator.Test.Agents;

public partial class AgentsTest : CalculatorTest {
    private List<DriveDisc> GetDriveDiscs(CalcRequest req) => req.Discs.Select((d, idx) =>
        new DriveDisc(
            d.SetId, 
            Convert.ToUInt32(idx), 
            d.Rarity, 
            Stat.Stats[d.Stats[0]], 
            d.StatsLevels
                .Skip(1)
                .Select(p => (p.Value, Stat.SubStats[p.Key])))
    ).ToList();
}