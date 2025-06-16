using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Domain.ValueObjects;
using R3;

namespace AttendanceRecord.Application.Services;

public class WorkRecordStore : IDisposable
{
    private readonly IntervalService _interval;
    private readonly WorkRecordFactory _workRecordFactory;
    private readonly WorkRecordService _workRecordService;
    private readonly CompositeDisposable _disposables = new();

    private readonly ReactiveProperty<WorkRecord> _workRecordToday = new();
    private readonly ReactiveProperty<WorkRecordTally> _workRecordTallyThisMonth = new();

    public ReadOnlyReactiveProperty<WorkRecord> WorkRecordToday => _workRecordToday;
    public ReadOnlyReactiveProperty<WorkRecordTally> WorkRecordTallyThisMonth => _workRecordTallyThisMonth;
    public Observable<WorkRecordResponseDto> WorkRecordTodayResponse { get; }
    public Observable<WorkRecordTallyResponseDto> WorkRecordTallyThisMonthResponse { get; }

    public WorkRecordStore(
        IntervalService interval, WorkRecordFactory workRecordFactory, WorkRecordService workRecordService)
    {
        _interval = interval;
        _workRecordFactory = workRecordFactory;
        _workRecordService = workRecordService;

        _workRecordToday.AddTo(_disposables);
        _workRecordTallyThisMonth.AddTo(_disposables);

        WorkRecordTodayResponse = _interval.OneSecondInterval
            .CombineLatest(_workRecordToday, (_, w) => WorkRecordResponseDto.FromDomain(w))
            .DistinctUntilChanged();
        WorkRecordTallyThisMonthResponse = _interval.OneSecondInterval
            .CombineLatest(_workRecordTallyThisMonth, (_, t) => WorkRecordTallyResponseDto.FromDomain(t))
            .DistinctUntilChanged();

        _interval.OneSecondInterval
            .Prepend(Unit.Default) // 初回ロードのためにUnit.Defaultを追加
            .SubscribeAwait(async (_, _) => await LoadAsync())
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();

    private async ValueTask LoadAsync(bool forceReload = false)
    {
        if (forceReload || _workRecordToday.Value.RecordedDate != DateTime.Today)
        {
            _workRecordToday.Value =
                await _workRecordFactory.FindByDateAsync(DateTime.Today)
                ?? Domain.Entities.WorkRecord.Empty;
        }

        _workRecordTallyThisMonth.Value =
           forceReload || _workRecordTallyThisMonth.Value.RecordedMonth != DateTime.Today.Month
               ? await _workRecordFactory.GetMonthlyTallyAsync(DateTime.Today)
               : _workRecordTallyThisMonth.Value.Recreate(_workRecordToday.Value);
    }

    public async Task ReloadAsync() => await LoadAsync(forceReload: true);
}
