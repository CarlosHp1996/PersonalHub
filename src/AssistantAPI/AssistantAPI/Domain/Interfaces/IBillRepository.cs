using AssistantAPI.Domain.Entities;
using AssistantAPI.Domain.Enums;

namespace AssistantAPI.Domain.Interfaces;

public interface IBillRepository
{
    Task<IEnumerable<Bill>> GetAllAsync(BillStatus? status = null);
    Task<Bill?> GetByIdAsync(Guid id);
    Task<Bill> CreateAsync(Bill bill);
    Task<Bill> UpdateAsync(Bill bill);
    Task DeleteAsync(Guid id);
}
