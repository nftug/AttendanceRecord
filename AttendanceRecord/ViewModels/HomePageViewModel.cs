using System.Text.Json;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Application.UseCases.WorkRecord;
using AttendanceRecord.Constants;
using AttendanceRecord.Dtos.HomePage;
using BrowserBridge;
using Mediator.Switch;
using R3;

namespace AttendanceRecord.ViewModels;

public sealed class HomePageViewModel(
    IEventDispatcher eventDispatcher,
    CurrentWorkRecordStateStore workRecordStore,
    ISender mediator
) : ViewModelBase<HomePageCommandType>(eventDispatcher)
{
    protected override void OnFirstRender()
    {
        workRecordStore.CurrentWorkRecordState
            .Select(state => new HomePageStateEvent(state))
            .Subscribe(stateEvent => Dispatch(stateEvent, AppJsonContext.Default.EventMessageCurrentWorkRecordStateDto))
            .AddTo(Disposable);
    }

    protected override ValueTask HandleActionAsync(HomePageCommandType action, JsonElement? payload, Guid? commandId)
        => action switch
        {
            HomePageCommandType.ToggleWork => ToggleWorkAsync(commandId),
            HomePageCommandType.ToggleRest => ToggleRestAsync(commandId),
            _ => throw new NotSupportedException($"Action '{action}' is not supported.")
        };

    private async ValueTask ToggleWorkAsync(Guid? commandId)
    {
        await mediator.Send(new ToggleWork());
        if (commandId != null)
            Dispatch(new ToggleWorkResultEvent(commandId.Value), BridgeJsonContext.Default.EventMessageDummyEventPayload);
    }

    private async ValueTask ToggleRestAsync(Guid? commandId)
    {
        await mediator.Send(new ToggleRest());
        if (commandId != null)
            Dispatch(new ToggleRestResultEvent(commandId.Value), BridgeJsonContext.Default.EventMessageDummyEventPayload);
    }
}
