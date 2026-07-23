using InterknotCalculator.Core.Classes.EtherVeils;

namespace InterknotCalculator.Core.Classes.Events;

public sealed class EventBus {
    public delegate void ActionExecutedEvent(Context ctx, ActionExecutedEventArgs evt);
    public delegate void AftershockEvent(Context ctx, ActionExecutedEventArgs evt);
    public delegate void AnomalyBuildupEvent(Context ctx, AnomalyBuildupEventArgs evt);
    public delegate void AnomalyThresholdEvent(Context ctx, AnomalyThresholdEventArgs evt);
    public delegate void AnomalyTriggeredEvent(Context ctx, AnomalyTriggeredEventArgs evt);
    public delegate void EtherVeilActivatedEvent(Context ctx, EtherVeilEventArgs evt);
    public delegate void EtherVeilDeactivatedEvent(Context ctx, EtherVeilEventArgs evt);
    
    public List<ActionExecutedEvent> OnActionExecuted { get; } = [];
    public List<AftershockEvent> OnAftershock { get; } = [];
    public List<AnomalyBuildupEvent> OnAnomalyBuildup { get; } = [];
    public List<AnomalyThresholdEvent> OnAnomalyThreshold { get; } = [];
    public List<AnomalyTriggeredEvent> OnAnomalyTriggered { get; } = [];
    public List<EtherVeilActivatedEvent> OnEtherVeilActivated { get; } = [];
    public List<EtherVeilDeactivatedEvent> OnEtherVeilDeactivated { get; } = [];

    public void ActionExecuted(Context ctx, ActionExecutedEventArgs e) {
        foreach (var actionExecutedEvent in OnActionExecuted) {
            actionExecutedEvent(ctx, e);
        }
    }
    public void Aftershock(Context ctx, ActionExecutedEventArgs e) {
        foreach (var aftershockEvent in OnAftershock) {
            aftershockEvent(ctx, e);
        }
    }
    public void AnomalyBuildup(Context ctx, AnomalyBuildupEventArgs e) {
        foreach (var anomalyBuildupEvent in OnAnomalyBuildup) {
            anomalyBuildupEvent(ctx, e);
        }
    }
    public void AnomalyThreshold(Context ctx, AnomalyThresholdEventArgs e) {
        foreach (var anomalyThresholdEvent in OnAnomalyThreshold) {
            anomalyThresholdEvent(ctx, e);
        }
    }
    public void AnomalyTriggered(Context ctx, AnomalyTriggeredEventArgs e) {
        foreach (var anomalyTriggeredEvent in OnAnomalyTriggered) {
            anomalyTriggeredEvent(ctx, e);
        }
    }
    public void EtherVeilActivated(Context ctx, EtherVeilEventArgs evt) {
        foreach (var etherVeilActivatedEvent in OnEtherVeilActivated) {
            etherVeilActivatedEvent(ctx, evt);
        }
    }
    public void EtherVeilDeactivated(Context ctx, EtherVeilEventArgs evt) {
        foreach (var etherVeilDeactivatedEvent in OnEtherVeilDeactivated) {
            etherVeilDeactivatedEvent(ctx, evt);
        }
    }
}