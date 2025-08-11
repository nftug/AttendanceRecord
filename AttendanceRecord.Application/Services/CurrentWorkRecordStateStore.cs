using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Domain.ValueObjects;
using R3;

namespace AttendanceRecord.Application.Services;

public class CurrentWorkRecordStateStore : IDisposable
{
    private readonly IntervalService _interval;
    private readonly WorkRecordFactory _workRecordFactory;
    private readonly CompositeDisposable _disposables = [];

    private readonly ReactiveProperty<WorkRecord> _workRecordToday = new(WorkRecord.Empty);
    private readonly ReactiveProperty<WorkRecordTally> _workRecordTallyThisMonth = new(WorkRecordTally.Empty);

    public ReadOnlyReactiveProperty<CurrentWorkRecordStateDto> CurrentWorkRecordState { get; }
    internal ReadOnlyReactiveProperty<WorkRecord> WorkRecordToday => _workRecordToday;

    public CurrentWorkRecordStateStore(IntervalService interval, WorkRecordFactory workRecordFactory)
    {
        _interval = interval;
        _workRecordFactory = workRecordFactory;

        _workRecordToday.AddTo(_disposables);
        _workRecordTallyThisMonth.AddTo(_disposables);

        CurrentWorkRecordState = _workRecordToday
            .CombineLatest(_workRecordTallyThisMonth, CurrentWorkRecordStateDto.FromDomain)
            .ToReadOnlyReactiveProperty(CurrentWorkRecordStateDto.Empty)
            .AddTo(_disposables);

        _interval.OneSecondInterval
            .Prepend(Unit.Default) // 初回ロードのためにUnit.Defaultを追加
            .SubscribeAwait(async (_, _) => await LoadAsync())
            .AddTo(_disposables);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposables.Dispose();
    }

    private async ValueTask LoadAsync(bool forceReload = false)
    {
        _workRecordToday.Value =
            forceReload || _workRecordToday.Value.RecordedDate != DateTime.Today
                ? await _workRecordFactory.FindByDateAsync(DateTime.Today)
                ?? WorkRecord.Empty
            : _workRecordToday.Value;

        _workRecordTallyThisMonth.Value =
           forceReload || _workRecordTallyThisMonth.Value.RecordedMonth != DateTime.Today.Month
               ? await _workRecordFactory.GetMonthlyTallyAsync(DateTime.Today.Year, DateTime.Today.Month)
               : _workRecordTallyThisMonth.Value.Recreate(_workRecordToday.Value);
    }

    public async Task ReloadAsync() => await LoadAsync(forceReload: true);
}
