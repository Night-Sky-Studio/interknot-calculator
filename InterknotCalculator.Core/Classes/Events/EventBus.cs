namespace InterknotCalculator.Core.Classes.Events;

public sealed class EventBus {
    public delegate void CalculationStartedEvent(Context ctx);
    public delegate void ActionExecutingEvent(Context ctx, ActionExecutingEventArgs evt);
    public delegate void ActionExecutedEvent(Context ctx, ActionExecutedEventArgs evt);
    public delegate void AftershockEvent(Context ctx, ActionExecutingEventArgs evt);
    public delegate void AnomalyBuildupEvent(Context ctx, AnomalyBuildupEventArgs evt);
    public delegate void AnomalyThresholdEvent(Context ctx, AnomalyThresholdEventArgs evt);
    public delegate void AnomalyTriggeredEvent(Context ctx, AnomalyTriggeredEventArgs evt);
    public delegate void EtherVeilActivatedEvent(Context ctx, EtherVeilEventArgs evt);
    public delegate void EtherVeilDeactivatedEvent(Context ctx, EtherVeilEventArgs evt);
    
    public event CalculationStartedEvent? OnCalculationStarted;
    public event ActionExecutingEvent? OnActionExecuting;
    public event ActionExecutedEvent? OnActionExecuted;
    public event AftershockEvent? OnAftershock;
    public event AnomalyBuildupEvent? OnAnomalyBuildup;
    public event AnomalyThresholdEvent? OnAnomalyThreshold;
    public event AnomalyTriggeredEvent? OnAnomalyTriggered;
    public event EtherVeilActivatedEvent? OnEtherVeilActivated;
    public event EtherVeilDeactivatedEvent? OnEtherVeilDeactivated;

    public void CalculationStarted(Context ctx) {
        OnCalculationStarted?.Invoke(ctx);
    }
    public void ActionExecuting(Context ctx, ActionExecutingEventArgs e) {
        OnActionExecuting?.Invoke(ctx, e);
    }
    public void ActionExecuted(Context ctx, ActionExecutedEventArgs e) {
        OnActionExecuted?.Invoke(ctx, e);
    }
    public void Aftershock(Context ctx, ActionExecutingEventArgs e) {
        OnAftershock?.Invoke(ctx, e);
    }
    public void AnomalyBuildup(Context ctx, AnomalyBuildupEventArgs e) {
        OnAnomalyBuildup?.Invoke(ctx, e);
    }
    public void AnomalyThreshold(Context ctx, AnomalyThresholdEventArgs e) {
        OnAnomalyThreshold?.Invoke(ctx, e);
    }
    public void AnomalyTriggered(Context ctx, AnomalyTriggeredEventArgs e) {
        OnAnomalyTriggered?.Invoke(ctx, e);
    }
    public void EtherVeilActivated(Context ctx, EtherVeilEventArgs evt) {
        OnEtherVeilActivated?.Invoke(ctx, evt);
    }
    public void EtherVeilDeactivated(Context ctx, EtherVeilEventArgs evt) {
        OnEtherVeilDeactivated?.Invoke(ctx, evt);
    }
}