using AttendanceRecord.Application.Services;
using AttendanceRecord.Presentation.Constants;
using AttendanceRecord.Presentation.Dtos.Alarm;
using BrowserBridge;
using R3;

namespace AttendanceRecord.Presentation.ViewModels;

public sealed class AlarmViewModel(IEventDispatcher dispatcher, WorkRecordAlarmService alarmService)
    : ViewModelBase<AlarmCommandType>(dispatcher)
{
    protected override void OnFirstRender()
    {
        alarmService.AlarmTriggeredCommand
            .Subscribe(v => Dispatch(new(v), AppJsonContext.Default.TriggeredEvent))
            .AddTo(Disposable);
    }

    protected override ValueTask HandleActionAsync(AlarmCommandType action, CommandMessage message) =>
        (action, message.Payload, message.CommandId) switch
        {
            (AlarmCommandType.Snooze, var payload, var commandId)
                when payload.TryParse(AppJsonContext.Default.SnoozeCommandPayload, out var req) =>
                    SnoozeAlarmAsync(req, commandId),
            _ => throw new NotImplementedException($"Action {action} is not implemented or payload is missing.")
        };

    private ValueTask SnoozeAlarmAsync(SnoozeCommandPayload payload, Guid? commandId)
    {
        alarmService.Snooze(payload.Type);
        Dispatch(new(commandId), AppJsonContext.Default.SnoozeResultEvent);
        return ValueTask.CompletedTask;
    }
}
