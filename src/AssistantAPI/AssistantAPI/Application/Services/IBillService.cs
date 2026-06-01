using AssistantAPI.Application.DTOs;
using AssistantAPI.Domain.Enums;

namespace AssistantAPI.Application.Services;

public interface IBillService
{
    Task<BillListResponse> GetAllAsync(BillStatus? status = null);
    Task<BillResponse?> GetByIdAsync(Guid id);
    Task<BillResponse> CreateAsync(CreateBillRequest request);
    Task<BillResponse?> UpdateAsync(Guid id, UpdateBillRequest request);
    Task<BillResponse?> MarkAsPaidAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
