using AttendanceRecord.Domain.Services;
using R3;

namespace AttendanceRecord.Application.Services;

public class WorkRecordAlarmService : IDisposable
{
    private readonly CurrentWorkRecordStateStore _currentWorkRecordStateStore;
    private readonly AppConfigStore _appConfigStore;
    private readonly CompositeDisposable _disposables = [];

    private readonly ReactiveCommand<AlarmType> _alarmCommand = new();

    public Observable<AlarmType> AlarmCommand => _alarmCommand;

    public WorkRecordAlarmService(
        CurrentWorkRecordStateStore currentWorkRecordStateStore,
        AppConfigStore appConfigStore
    )
    {
        _currentWorkRecordStateStore = currentWorkRecordStateStore;
        _appConfigStore = appConfigStore;

        // 退勤前アラーム
        _currentWorkRecordStateStore.WorkRecordToday
            .Select(_appConfigStore.Config.WorkTimeAlarm.ShouldTrigger)
            .DistinctUntilChanged()
            .Where(triggered => triggered)
            .Subscribe(_ => _alarmCommand.Execute(AlarmType.WorkTimeAlarm))
            .AddTo(_disposables);

        // 休憩前アラーム
        _currentWorkRecordStateStore.WorkRecordToday
            .Select(_appConfigStore.Config.RestTimeAlarm.ShouldTrigger)
            .DistinctUntilChanged()
            .Where(triggered => triggered)
            .Subscribe(_ => _alarmCommand.Execute(AlarmType.RestTimeAlarm))
            .AddTo(_disposables);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposables.Dispose();
    }
}

public enum AlarmType
{
    WorkTimeAlarm,
    RestTimeAlarm
}
