using System.Text.Json;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Application.UseCases.WorkRecord;
using AttendanceRecord.Constants;
using AttendanceRecord.Dtos.HomePage;
using BrowserBridge;
using R3;

namespace AttendanceRecord.ViewModels;

public class HomePageViewModel(
    IEventDispatcher eventDispatcher,
    CurrentWorkRecordStateStore workRecordStore,
    ToggleWorkUseCase toggleWorkUseCase,
    ToggleRestUseCase toggleRestUseCase
) : ViewModelBase<HomePageCommandType>(eventDispatcher)
{
    protected override ValueTask HandleActionAsync(HomePageCommandType action, JsonElement? payload, Guid? commandId)
        => action switch
        {
            HomePageCommandType.ToggleWork => ToggleWorkAsync(commandId),
            HomePageCommandType.ToggleRest => ToggleRestAsync(commandId),
            _ => throw new NotSupportedException($"Action '{action}' is not supported.")
        };

    protected override void OnFirstRender()
    {
        workRecordStore.CurrentWorkRecordState
            .Select(state => new CurrentWorkRecordStateEvent(state))
            .Subscribe(stateEvent => Dispatch(stateEvent, AppJsonContext.Default.EventMessageCurrentWorkRecordStateDto))
            .AddTo(Disposable);
    }

    private async ValueTask ToggleWorkAsync(Guid? commandId)
    {
        if (commandId == null)
            throw new ArgumentNullException(nameof(commandId));

        await toggleWorkUseCase.ExecuteAsync();
        Dispatch(new ToggleWorkEvent(commandId.Value), BridgeJsonContext.Default.EventMessageDummyEventPayload);
    }

    private async ValueTask ToggleRestAsync(Guid? commandId)
    {
        if (commandId == null)
            throw new ArgumentNullException(nameof(commandId));

        await toggleRestUseCase.ExecuteAsync();
        Dispatch(new ToggleRestEvent(commandId.Value), BridgeJsonContext.Default.EventMessageDummyEventPayload);
    }
}
