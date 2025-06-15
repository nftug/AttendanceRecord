using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.Interfaces;

public interface IWorkRecordRepository
{
    Task SaveAsync(WorkRecord workRecord);
    Task DeleteAsync(Guid id);
    Task<WorkRecord?> FindByIdAsync(Guid id);
    Task<WorkRecord?> FindByDateAsync(DateTime date);
    Task<IReadOnlyList<WorkRecord>> FindByMonthAsync(DateTime month);
}
