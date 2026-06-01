namespace AssistantAPI.Application.DTOs;

public record CreateBillRequest(string Name, decimal Amount, DateOnly DueDate, string? Notes);

public record UpdateBillRequest(string Name, decimal Amount, DateOnly DueDate, string? Notes);

public record BillResponse(Guid Id, string Name, decimal Amount, DateOnly DueDate,
    string Status, DateTime? PaidAt, string? Notes, bool IsOverdue, int DaysUntilDue);

public record BillListResponse(IEnumerable<BillResponse> Data, BillSummary Summary);

public record BillSummary(int TotalPending, int TotalOverdue, decimal TotalAmountDue);
