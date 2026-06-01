using AssistantAPI.Application.DTOs;
using AssistantAPI.Domain.Entities;
using AssistantAPI.Domain.Enums;
using AssistantAPI.Domain.Interfaces;

namespace AssistantAPI.Application.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _repository;

    public BillService(IBillRepository repository)
    {
        _repository = repository;
    }

    public async Task<BillListResponse> GetAllAsync(BillStatus? status = null)
    {
        var bills = await _repository.GetAllAsync(status);
        
        var responseData = bills.Select(MapToResponse).ToList();

        var pendingBills = bills.Where(b => b.Status == BillStatus.Pending).ToList();
        var overdueBills = bills.Where(b => b.Status == BillStatus.Overdue || (b.Status == BillStatus.Pending && b.IsOverdue)).ToList();
        
        var summary = new BillSummary(
            TotalPending: pendingBills.Count,
            TotalOverdue: overdueBills.Count,
            TotalAmountDue: pendingBills.Sum(b => b.Amount) + overdueBills.Where(b => !pendingBills.Contains(b)).Sum(b => b.Amount)
        );

        return new BillListResponse(responseData, summary);
    }

    public async Task<BillResponse?> GetByIdAsync(Guid id)
    {
        var bill = await _repository.GetByIdAsync(id);
        return bill is null ? null : MapToResponse(bill);
    }

    public async Task<BillResponse> CreateAsync(CreateBillRequest request)
    {
        var bill = new Bill
        {
            Name = request.Name,
            Amount = request.Amount,
            DueDate = request.DueDate,
            Notes = request.Notes
        };

        var created = await _repository.CreateAsync(bill);
        return MapToResponse(created);
    }

    public async Task<BillResponse?> UpdateAsync(Guid id, UpdateBillRequest request)
    {
        var bill = await _repository.GetByIdAsync(id);
        if (bill is null) return null;

        bill.Name = request.Name;
        bill.Amount = request.Amount;
        bill.DueDate = request.DueDate;
        bill.Notes = request.Notes;

        var updated = await _repository.UpdateAsync(bill);
        return MapToResponse(updated);
    }

    public async Task<BillResponse?> MarkAsPaidAsync(Guid id)
    {
        var bill = await _repository.GetByIdAsync(id);
        if (bill is null) return null;

        bill.MarkAsPaid();
        var updated = await _repository.UpdateAsync(bill);
        
        return MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bill = await _repository.GetByIdAsync(id);
        if (bill is null) return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    private static BillResponse MapToResponse(Bill bill)
    {
        return new BillResponse(
            Id: bill.Id,
            Name: bill.Name,
            Amount: bill.Amount,
            DueDate: bill.DueDate,
            Status: bill.Status.ToString(),
            PaidAt: bill.PaidAt,
            Notes: bill.Notes,
            IsOverdue: bill.IsOverdue,
            DaysUntilDue: bill.DaysUntilDue
        );
    }
}
