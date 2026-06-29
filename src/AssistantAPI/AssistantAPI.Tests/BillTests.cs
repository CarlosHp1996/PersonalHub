using AssistantAPI.Domain.Entities;
using AssistantAPI.Domain.Enums;
using FluentAssertions;

namespace AssistantAPI.Tests;

public class BillTests
{
    [Fact]
    public void MarkAsPaid_ShouldSetStatusAndPaidAt()
    {
        // Arrange
        var bill = new Bill { DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)) };

        // Act
        bill.MarkAsPaid();

        // Assert
        bill.Status.Should().Be(BillStatus.Paid);
        bill.PaidAt.Should().NotBeNull();
    }

    [Fact]
    public void IsOverdue_ShouldReturnTrue_WhenPendingAndDueDatePast()
    {
        var bill = new Bill { Status = BillStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) };
        bill.IsOverdue.Should().BeTrue();
    }

    [Fact]
    public void IsOverdue_ShouldReturnFalse_WhenPaid()
    {
        var bill = new Bill { Status = BillStatus.Paid, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) };
        bill.IsOverdue.Should().BeFalse();
    }
}
