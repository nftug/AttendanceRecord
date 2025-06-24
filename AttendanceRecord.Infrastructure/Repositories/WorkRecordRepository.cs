using System.Text.Json;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Infrastructure.Constants;
using AttendanceRecord.Infrastructure.Dtos;
using AttendanceRecord.Infrastructure.Services;

namespace AttendanceRecord.Infrastructure.Repositories;

public class WorkRecordRepository : IWorkRecordRepository, IDisposable
{
    private readonly string _filePath;
    private readonly List<WorkRecord> _workRecords;
    private readonly FileStream _lockStream;
    private readonly Lock _syncRoot = new();

    public WorkRecordRepository(AppDataDirectoryService appDataDirectory)
    {
        _filePath = appDataDirectory.GetFilePath("work_records.json");
        // プロセス生存期間中、ファイルを排他ロックで開きっぱなしにする
        _lockStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        _workRecords = LoadFile(_lockStream);
    }

    private static List<WorkRecord> LoadFile(FileStream stream)
    {
        if (stream.Length == 0) return [];
        stream.Position = 0;
        var dtos = JsonSerializer.Deserialize(stream, JsonContext.Default.WorkRecordFileDtoArray);
        return dtos?.Select(x => x.ToDomain()).ToList() ?? [];
    }

    public void Dispose() => _lockStream?.Dispose();

    public ValueTask<WorkRecord?> FindByDateAsync(DateTime date)
        => new(_workRecords.FirstOrDefault(x => x.Duration.StartedOn.Date == date.Date));

    public ValueTask<WorkRecord?> FindByIdAsync(Guid id)
        => new(_workRecords.FirstOrDefault(x => x.Id == id));

    public ValueTask<IReadOnlyList<WorkRecord>> FindByMonthAsync(DateTime month)
    {
        var firstDay = new DateTime(month.Year, month.Month, 1);
        var nextMonth = firstDay.AddMonths(1);
        var result = _workRecords
            .Where(x => x.Duration.StartedOn >= firstDay && x.Duration.StartedOn < nextMonth)
            .ToList();
        return new(result);
    }

    public ValueTask SaveAsync(WorkRecord workRecord)
    {
        lock (_syncRoot)
        {
            var idx = _workRecords.FindIndex(x => x.Id == workRecord.Id);
            if (idx >= 0)
                _workRecords[idx] = workRecord;
            else
                _workRecords.Add(workRecord);
            SaveAll();
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask DeleteAsync(Guid id)
    {
        lock (_syncRoot)
        {
            _workRecords.RemoveAll(x => x.Id == id);
            SaveAll();
        }

        return ValueTask.CompletedTask;
    }

    private void SaveAll()
    {
        _lockStream.SetLength(0);
        _lockStream.Position = 0;
        var dtos = _workRecords.Select(WorkRecordFileDto.FromDomain).ToArray();
        JsonSerializer.Serialize(_lockStream, dtos, JsonContext.Default.WorkRecordFileDtoArray);
        _lockStream.Flush();
    }
}
