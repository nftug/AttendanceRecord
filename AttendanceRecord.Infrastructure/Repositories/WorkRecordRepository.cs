using System.Text.Json;
using System.Text.Json.Serialization;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Infrastructure.Constants;
using AttendanceRecord.Infrastructure.Dtos;
using AttendanceRecord.Infrastructure.Services;

namespace AttendanceRecord.Infrastructure.Repositories;

public class WorkRecordRepository(AppDataDirectoryService appDataDirectory) : IWorkRecordRepository
{
    private readonly string _filePath = appDataDirectory.GetFilePath("work_records.json");

    private async Task<List<WorkRecordFileDto>> LoadAllAsync()
    {
        if (!File.Exists(_filePath)) return [];
        using var stream = File.OpenRead(_filePath);
        var dtos = await JsonSerializer.DeserializeAsync(
            stream,
            JsonContext.Default.WorkRecordFileDtoArray
        );
        return dtos?.ToList() ?? [];
    }

    private async Task SaveAllAsync(List<WorkRecordFileDto> dtos)
    {
        using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(
            stream,
            dtos.ToArray(),
            JsonContext.Default.WorkRecordFileDtoArray
        );
    }

    public async Task<WorkRecord?> FindByDateAsync(DateTime date)
    {
        var all = await LoadAllAsync();
        var dto = all.FirstOrDefault(x => x.Duration.StartedOn.Date == date.Date);
        return dto?.ToDomain();
    }

    public async Task<WorkRecord?> FindByIdAsync(Guid id)
    {
        var all = await LoadAllAsync();
        var dto = all.FirstOrDefault(x => x.Id == id);
        return dto?.ToDomain();
    }

    public async Task<IReadOnlyList<WorkRecord>> FindByMonthAsync(DateTime month)
    {
        var all = await LoadAllAsync();
        var firstDay = new DateTime(month.Year, month.Month, 1);
        var nextMonth = firstDay.AddMonths(1);
        return all
            .Where(x => x.Duration.StartedOn >= firstDay && x.Duration.StartedOn < nextMonth)
            .Select(x => x.ToDomain())
            .ToList();
    }

    public async Task SaveAsync(WorkRecord workRecord)
    {
        var all = await LoadAllAsync();
        var idx = all.FindIndex(x => x.Id == workRecord.Id);
        var dto = WorkRecordFileDto.FromDomain(workRecord);
        if (idx >= 0)
            all[idx] = dto;
        else
            all.Add(dto);
        await SaveAllAsync(all);
    }

    public async Task DeleteAsync(Guid id)
    {
        var all = await LoadAllAsync();
        all.RemoveAll(x => x.Id == id);
        await SaveAllAsync(all);
    }
}
