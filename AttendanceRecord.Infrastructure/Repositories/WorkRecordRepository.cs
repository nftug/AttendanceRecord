using System.Text.Json;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Infrastructure.Constants;
using AttendanceRecord.Infrastructure.Dtos;
using AttendanceRecord.Infrastructure.Services;

namespace AttendanceRecord.Infrastructure.Repositories;

public class WorkRecordRepository(AppDataDirectoryService appDataDirectory) : IWorkRecordRepository
{
    private readonly string _filePath = appDataDirectory.GetFilePath("work_records.json");

    public async ValueTask<WorkRecord?> FindByDateAsync(DateTime date)
        => (await LoadAsync()).FirstOrDefault(x => x.RecordedDate.Date == date.Date);

    public async ValueTask<WorkRecord?> FindByIdAsync(Guid id)
        => (await LoadAsync()).FirstOrDefault(x => x.Id == id);

    public async ValueTask<IReadOnlyList<WorkRecord>> FindByMonthAsync(DateTime date)
    {
        var firstDay = new DateTime(date.Year, date.Month, 1);
        var nextMonth = firstDay.AddMonths(1);
        return [.. (await LoadAsync())
            .Where(x => x.Duration.StartedOn >= firstDay && x.Duration.StartedOn < nextMonth)];
    }

    public async ValueTask SaveAsync(WorkRecord workRecord)
    {
        var items = await LoadAsync();
        items.RemoveAll(x => x.Id == workRecord.Id);
        items.Add(workRecord);
        await SaveAsync(items);
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        var items = await LoadAsync();
        items.RemoveAll(x => x.Id == id);
        await SaveAsync(items);
    }

    private async ValueTask<List<WorkRecord>> LoadAsync()
    {
        try
        {
            using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (stream.Length == 0) return [];
            var content = await JsonSerializer.DeserializeAsync(stream, InfrastructureJsonContext.Indented.WorkRecordFileDtoArray);
            return content?.Select(x => x.ToDomain()).ToList() ?? [];
        }
        catch (FileNotFoundException)
        {
            return [];
        }
    }

    private async ValueTask SaveAsync(IEnumerable<WorkRecord> workRecords)
    {
        var dtoArray = workRecords.Select(WorkRecordFileDto.FromDomain).ToArray();
        using var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, dtoArray, InfrastructureJsonContext.Indented.WorkRecordFileDtoArray);
        await stream.FlushAsync();
    }
}
