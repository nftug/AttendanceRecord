using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Enums;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Domain.ValueObjects;
using R3;

namespace AttendanceRecord.Application.Services;

public sealed class WorkRecordAlarmService : IDisposable
{
    private readonly AppConfigStore _appConfigStore;
    private readonly CompositeDisposable _disposables = [];

    private readonly ReactiveProperty<WorkEndAlarm> _workEndAlarm = new(new());
    private readonly ReactiveProperty<RestStartAlarm> _restStartAlarm = new(new());
    private readonly ReactiveCommand<AlarmResponseDto> _alarmTriggeredCommand = new();

    public Observable<AlarmResponseDto> AlarmTriggered => _alarmTriggeredCommand;

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
            .Delay(TimeSpan.FromMilliseconds(100)) // 初期化後にトリガーされるのを防ぐ
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

    public void Dispose() => _disposables.Dispose();
}

file record AlarmTriggered(AlarmType Type, bool Triggered);
