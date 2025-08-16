namespace AttendanceRecord.Domain.Interfaces;

public interface ISingleInstanceGuard : IDisposable
{
    bool TryAcquireLock();
}
