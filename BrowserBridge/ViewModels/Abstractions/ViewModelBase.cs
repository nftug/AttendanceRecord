using System.Text.Json.Serialization.Metadata;
using R3;

namespace BrowserBridge;

public abstract class ViewModelBase<TCommandType> : DisposableBase, IViewModel
    where TCommandType : struct, Enum
{
    private readonly IEventDispatcher _dispatcher;

    private readonly ReactiveProperty<Guid> _viewId = new();
    protected ReadOnlyReactiveProperty<Guid> ViewId => _viewId;

    public ViewModelBase(IEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _viewId.AddTo(Disposable);

        _viewId.Where(v => v != Guid.Empty)
            .Take(1)
            .Subscribe(_ => OnFirstRender())
            .AddTo(Disposable);
    }

    protected virtual void OnFirstRender() { }

    public ValueTask HandleAsync(CommandMessage message)
        => Enum.TryParse<TCommandType>(message.Command, true, out var action)
            ? HandleActionAsync(action, message)
            : ValueTask.CompletedTask;

    protected void Dispatch<TEventMessage>(TEventMessage message)
        where TEventMessage : EventMessageBase
        => _dispatcher.Dispatch(message with { ViewId = _viewId.Value });

    protected void Dispatch<TEventMessage>(TEventMessage message, JsonTypeInfo<TEventMessage> jsonTypeInfo)
        where TEventMessage : EventMessageBase
        => _dispatcher.Dispatch(message with { ViewId = _viewId.Value }, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TCommandType action, CommandMessage message);

    public void SetViewId(Guid viewId)
    {
        if (viewId == Guid.Empty)
            throw new ArgumentException("ViewId cannot be empty.", nameof(viewId));
        if (_viewId.Value != Guid.Empty)
            throw new InvalidOperationException("ViewId has already been set.");

        _viewId.Value = viewId;
    }
}
