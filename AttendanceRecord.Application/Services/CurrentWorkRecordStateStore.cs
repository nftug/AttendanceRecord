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

    private readonly ReactiveProperty<WorkRecord> _workRecordToday = new();
    private readonly ReactiveProperty<WorkRecordTally> _workRecordTallyThisMonth = new();

    public ReadOnlyReactiveProperty<CurrentWorkRecordStateDto> CurrentWorkRecordState { get; }

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
        if (forceReload || _workRecordToday.Value?.RecordedDate != DateTime.Today)
        {
            _workRecordToday.Value =
                await _workRecordFactory.FindByDateAsync(DateTime.Today)
                ?? WorkRecord.Empty;
        }

        _workRecordTallyThisMonth.Value =
           forceReload || _workRecordTallyThisMonth.Value?.RecordedMonth != DateTime.Today.Month
               ? await _workRecordFactory.GetMonthlyTallyAsync(DateTime.Today)
               : _workRecordTallyThisMonth.Value.Recreate(_workRecordToday.Value);
    }

    public async Task ReloadAsync() => await LoadAsync(forceReload: true);
}
