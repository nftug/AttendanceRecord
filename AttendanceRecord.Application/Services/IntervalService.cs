using R3;

namespace AttendanceRecord.Application.Services;

public class IntervalService
{
    public Observable<Unit> OneSecondInterval { get; }
        = Observable.Interval(TimeSpan.FromSeconds(1)).Publish().RefCount();
}
