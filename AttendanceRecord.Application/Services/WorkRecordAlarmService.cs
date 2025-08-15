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
    private readonly ReactiveCommand _workEndAlarmTriggerCommand = new();
    private readonly ReactiveCommand _restStartAlarmTriggerCommand = new();

    public Observable<Unit> WorkEndAlarmTriggeredCommand => _workEndAlarmTriggerCommand;
    public Observable<Unit> RestStartAlarmTriggeredCommand => _restStartAlarmTriggerCommand;

    public WorkRecordAlarmService(
        CurrentWorkRecordStateStore currentWorkRecordStateStore, AppConfigStore appConfigStore)
    {
        _appConfigStore = appConfigStore;

        _workEndAlarm.AddTo(_disposables);
        _restStartAlarm.AddTo(_disposables);
        _workEndAlarmTriggerCommand.AddTo(_disposables);
        _restStartAlarmTriggerCommand.AddTo(_disposables);

        currentWorkRecordStateStore.WorkRecordToday
            .Subscribe(wr =>
            {
                _workEndAlarm.Value = _workEndAlarm.Value.TryTrigger(wr, appConfigStore.Config);
                _restStartAlarm.Value = _restStartAlarm.Value.TryTrigger(wr, appConfigStore.Config);
            })
            .AddTo(_disposables);

        // 退勤前アラーム
        _workEndAlarm
            .Select(a => a.IsTriggered)
            .DistinctUntilChanged()
            .Where(triggered => triggered)
            .Subscribe(_ => _workEndAlarmTriggerCommand.Execute(Unit.Default))
            .AddTo(_disposables);

        // 休憩前アラーム
        _restStartAlarm
            .Select(a => a.IsTriggered)
            .DistinctUntilChanged()
            .Where(triggered => triggered)
            .Subscribe(_ => _restStartAlarmTriggerCommand.Execute(Unit.Default))
            .AddTo(_disposables);
    }

    public void SnoozeWorkEndAlarm()
    {
        _workEndAlarm.Value = _workEndAlarm.Value.MarkSnooze(_appConfigStore.Config);
    }

    public void SnoozeRestStartAlarm()
    {
        _restStartAlarm.Value = _restStartAlarm.Value.MarkSnooze(_appConfigStore.Config);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposables.Dispose();
    }
}
