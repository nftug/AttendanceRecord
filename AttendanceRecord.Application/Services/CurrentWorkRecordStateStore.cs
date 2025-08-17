using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Domain.Utils;
using AttendanceRecord.Domain.ValueObjects;
using R3;

namespace AttendanceRecord.Application.Services;

public class CurrentWorkRecordStateStore : IDisposable
{
    private readonly IntervalService _interval;
    private readonly IWorkRecordRepository _repository;
    private readonly CompositeDisposable _disposables = [];

    private readonly ReactiveProperty<WorkRecord> _workRecordToday = new(WorkRecord.Empty);
    private readonly ReactiveProperty<WorkRecordTally> _workRecordTallyThisMonth = new(WorkRecordTally.Empty);

    public ReadOnlyReactiveProperty<CurrentWorkRecordStateDto> CurrentWorkRecordState { get; }
    internal ReadOnlyReactiveProperty<WorkRecord> WorkRecordToday => _workRecordToday;

    public CurrentWorkRecordStateStore(
        IntervalService interval,
        IWorkRecordRepository repository,
        AppConfigStore appConfigStore)
    {
        _interval = interval;
        _repository = repository;

        _workRecordToday.AddTo(_disposables);
        _workRecordTallyThisMonth.AddTo(_disposables);

        CurrentWorkRecordState = _workRecordToday
            .CombineLatest(_workRecordTallyThisMonth,
                (wr, t) => CurrentWorkRecordStateDto.FromDomain(wr, t, appConfigStore.Config))
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
        var today = DateTimeProvider.Today;

        _workRecordToday.Value =
            forceReload || _workRecordToday.Value.RecordedDate != today
                ? await _repository.FindByDateAsync(today)
                ?? WorkRecord.Empty
            : _workRecordToday.Value.Recreate();

        // 月次の集計はWorkRecordの状態が変わるか、月が変わるまで更新しない
        if (forceReload ||
            _workRecordTallyThisMonth.Value.RecordedDate != today)
        {
            // 月次の集計を更新
            _workRecordTallyThisMonth.Value = new WorkRecordTally(
                await _repository.FindByMonthAsync(today));
        }
        {
            // 今日のレコードを追加して、月次の集計を更新
            var records = await _repository.FindByMonthAsync(today);
            var recordsWithToday = records.Append(_workRecordToday.Value).DistinctBy(x => x.RecordedDate);

            _workRecordTallyThisMonth.Value = new(recordsWithToday);
        }
    }

    public async Task ReloadAsync() => await LoadAsync(forceReload: true);
}
