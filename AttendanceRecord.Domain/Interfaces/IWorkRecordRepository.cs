using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Domain.Interfaces;

public interface IWorkRecordRepository
{
    ValueTask SaveAsync(WorkRecord workRecord);
    ValueTask DeleteAsync(Guid id);
    ValueTask<WorkRecord?> FindByIdAsync(Guid id);
    ValueTask<WorkRecord?> FindByDateAsync(DateOnly date);
    ValueTask<IReadOnlyList<WorkRecord>> FindByMonthAsync(YearAndMonth yearAndMonth);
}
