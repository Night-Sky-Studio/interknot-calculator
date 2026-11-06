using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test;

[TestFixture]
public class EtherVeilTests {
    private Context Context { get; set; }
    
    [SetUp]
    public void SetUp() {
        Context = new();
    }

    [Test]
    public async Task EtherVeilActivated() {
        var zhao = new Zhao();
        zhao.RegisterHooks(Context);
        
        var task = new TaskCompletionSource<EtherVeil>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await using var registration = cts.Token.Register(() => task.TrySetException(new TimeoutException()));
        
        Context.Events.OnEtherVeilActivated += (_, e) => {
           Assert.That(e.Agent, Is.EqualTo(zhao));
           
           task.TrySetResult(e.EtherVeil);
        };
        
        zhao.GetActionDamage(Context, new(SkillTag.Entry, "burst_of_frost"));

        var veil = await task.Task;
        
        Assert.That(veil, Is.Not.Null);
        Assert.That(veil, Is.TypeOf<Wellspring>());
    }

    [Test]
    public async Task MultipleVeilsActivated() {
        var zhao = new Zhao();
        zhao.RegisterHooks(Context);
        
        var sunna = new Sunna();
        sunna.RegisterHooks(Context);
        
        var task = new TaskCompletionSource<EtherVeil[]>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await using var registration = cts.Token.Register(() => task.TrySetException(new TimeoutException()));

        Context.Events.OnEtherVeilActivated += (c, _) => {
            if (c.GetEtherVeil<Wellspring>() is { } wellspring && 
                c.GetEtherVeil<DelusionReprise>() is { } reprise) {
                task.SetResult([wellspring, reprise]);
            }
        };
        
        zhao.GetActionDamage(Context, new(SkillTag.Entry, "burst_of_frost"));
        sunna.GetActionDamage(Context, new(SkillTag.ExSpecial, "special_photography_technique"));

        var veils = await task.Task;
        
        Assert.That(veils, Has.Exactly(2).Items);
        Assert.That(veils, Has.Exactly(1).TypeOf<Wellspring>());
        Assert.That(veils, Has.Exactly(1).TypeOf<DelusionReprise>());
    }

    [Test]
    public async Task EtherVeilAffectsStats() {
        var sunna = new Sunna();
        Context.Team.Add(sunna.Id, sunna);
        sunna.RegisterHooks(Context);
        
        var task = new TaskCompletionSource<EtherVeil>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await using var registration = cts.Token.Register(() => task.TrySetException(new TimeoutException()));
        
        Context.Events.OnEtherVeilActivated += (_, e) => {
            Assert.That(e.Agent, Is.EqualTo(sunna));
           
            task.TrySetResult(e.EtherVeil);
        };

        var statsBefore = sunna.CollectStats();
        
        sunna.GetActionDamage(Context, new(SkillTag.ExSpecial, "special_photography_technique"));
        
        var veil = await task.Task;
        
        Assert.That(veil, Is.Not.Null);
        Assert.That(veil, Is.TypeOf<DelusionReprise>());
        
        var statsAfter = sunna.CollectStats();
        
        Assert.That(statsAfter[Affix.Atk], Is.EqualTo(statsBefore[Affix.Atk] + 50));
    }
}