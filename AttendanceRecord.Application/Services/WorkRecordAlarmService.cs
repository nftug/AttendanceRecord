using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Enums;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Domain.ValueObjects;
using R3;

namespace AttendanceRecord.Application.Services;

public class WorkRecordAlarmService : IDisposable
{
    private readonly AppConfigStore _appConfigStore;
    private readonly CompositeDisposable _disposables = [];

    private readonly ReactiveProperty<WorkEndAlarm> _workEndAlarm = new();
    private readonly ReactiveProperty<RestStartAlarm> _restStartAlarm = new();
    private readonly ReactiveCommand<AlarmResponseDto> _alarmTriggeredCommand = new();

    public Observable<AlarmResponseDto> AlarmTriggeredCommand => _alarmTriggeredCommand;

    public WorkRecordAlarmService(
        CurrentWorkRecordStateStore currentWorkRecordStateStore, AppConfigStore appConfigStore)
    {
        _appConfigStore = appConfigStore;

        _workEndAlarm.AddTo(_disposables);
        _restStartAlarm.AddTo(_disposables);
        _alarmTriggeredCommand.AddTo(_disposables);

        currentWorkRecordStateStore.WorkRecordToday
            .Subscribe(wr =>
            {
                _workEndAlarm.Value = _workEndAlarm.Value.TryTrigger(wr, appConfigStore.Config);
                _restStartAlarm.Value = _restStartAlarm.Value.TryTrigger(wr, appConfigStore.Config);
            })
            .AddTo(_disposables);

        Observable
            .Merge(
                _workEndAlarm.Select(a => new AlarmTriggered(AlarmType.WorkEnd, a.IsTriggered)),
                _restStartAlarm.Select(a => new AlarmTriggered(AlarmType.RestStart, a.IsTriggered))
            )
            .DistinctUntilChanged()
            .Where(x => x.Triggered)
            .Subscribe(x => _alarmTriggeredCommand.Execute(new AlarmResponseDto(x.Type)))
            .AddTo(_disposables);
    }

    public void Snooze(AlarmType alarmType)
    {
        switch (alarmType)
        {
            case AlarmType.WorkEnd:
                _workEndAlarm.Value = _workEndAlarm.Value.MarkSnooze(_appConfigStore.Config);
                break;
            case AlarmType.RestStart:
                _restStartAlarm.Value = _restStartAlarm.Value.MarkSnooze(_appConfigStore.Config);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(alarmType), alarmType, null);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposables.Dispose();
    }
}

file record AlarmTriggered(AlarmType Type, bool Triggered);
