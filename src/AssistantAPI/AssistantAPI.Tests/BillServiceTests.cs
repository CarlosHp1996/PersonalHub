using AssistantAPI.Application.DTOs;
using AssistantAPI.Application.Services;
using AssistantAPI.Domain.Entities;
using AssistantAPI.Domain.Enums;
using AssistantAPI.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace AssistantAPI.Tests;

public class BillServiceTests
{
    private readonly Mock<IBillRepository> _repoMock;
    private readonly BillService _sut;

    public BillServiceTests()
    {
        _repoMock = new Mock<IBillRepository>();
        _sut = new BillService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnBillListResponse_WithCorrectSummary()
    {
        // Arrange
        var bills = new List<Bill>
        {
            new Bill { Amount = 100, Status = BillStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)) },
            new Bill { Amount = 50, Status = BillStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) } // overdue
        };
        _repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(bills);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Data.Should().HaveCount(2);
        result.Summary.TotalPending.Should().Be(2); // One is pending, one is technically pending but overdue
        result.Summary.TotalOverdue.Should().Be(1);
        result.Summary.TotalAmountDue.Should().Be(150);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepository_AndReturnDto()
    {
        var req = new CreateBillRequest("Test", 100, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), null);
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Bill>())).ReturnsAsync(new Bill { Name = "Test", Amount = 100 });

        var result = await _sut.CreateAsync(req);

        _repoMock.Verify(r => r.CreateAsync(It.Is<Bill>(b => b.Name == "Test")), Times.Once);
        result.Name.Should().Be("Test");
    }

    [Fact]
    public async Task MarkAsPaidAsync_ShouldReturnNull_WhenBillNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Bill?)null);
        var result = await _sut.MarkAsPaidAsync(Guid.NewGuid());
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Bill?)null);
        var result = await _sut.DeleteAsync(Guid.NewGuid());
        result.Should().BeFalse();
    }
}
