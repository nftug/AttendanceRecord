using AttendanceRecord.Domain.Interfaces;

namespace AttendanceRecord.Infrastructure.Services;

public sealed class FileLockSingleInstanceGuard(AppDataDirectoryService appData) : ISingleInstanceGuard
{
    private readonly string _lockPath = appData.GetFilePath("app.lock");
    private FileStream? _stream;

    public bool TryAcquireLock()
    {
        try
        {
            _stream = new(_lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }

    public void Dispose()
    {
        try { _stream?.Dispose(); } catch { }
        _stream = null;
        try { if (File.Exists(_lockPath)) File.Delete(_lockPath); } catch { }
    }
}
