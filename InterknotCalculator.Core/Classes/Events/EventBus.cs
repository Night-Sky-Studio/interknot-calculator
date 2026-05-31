namespace InterknotCalculator.Core.Classes.Events;

public sealed class EventBus {
    public delegate void ActionExecutedEvent(Context ctx, ActionExecutedEventArgs evt);
    public delegate void AftershockEvent(Context ctx, ActionExecutedEventArgs evt);
    public delegate void AnomalyBuildupEvent(Context ctx, AnomalyBuildupEventArgs evt);
    public delegate void AnomalyThresholdEvent(Context ctx, AnomalyThresholdEventArgs evt);
    public delegate void AnomalyTriggeredEvent(Context ctx, AnomalyTriggeredEventArgs evt);
    
    public List<ActionExecutedEvent> OnActionExecuted { get; } = [];
    public List<AftershockEvent> OnAftershock { get; } = [];
    public List<AnomalyBuildupEvent> OnAnomalyBuildup { get; } = [];
    public List<AnomalyThresholdEvent> OnAnomalyThreshold { get; } = [];
    public List<AnomalyTriggeredEvent> OnAnomalyTriggered { get; } = [];

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
}