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
            .Subscribe(state => Dispatch(new(state), AppJsonContext.Default.HomePageStateEvent))
            .AddTo(Disposable);
    }

    protected override ValueTask HandleActionAsync(HomePageCommandType action, JsonElement? payload, Guid? commandId)
        => action switch
        {
            HomePageCommandType.ToggleWork => ToggleWorkAsync(commandId),
            HomePageCommandType.ToggleRest => ToggleRestAsync(commandId),
            _ => throw new NotImplementedException($"Action {action} is not implemented.")
        };

    private async ValueTask ToggleWorkAsync(Guid? commandId)
    {
        var result = await mediator.Send(new ToggleWork());
        if (commandId != null)
            Dispatch(new(result, commandId.Value), AppJsonContext.Default.ToggleWorkResultEvent);
    }

    private async ValueTask ToggleRestAsync(Guid? commandId)
    {
        var result = await mediator.Send(new ToggleRest());
        if (commandId != null)
            Dispatch(new(result, commandId.Value), AppJsonContext.Default.ToggleRestResultEvent);
    }
}
