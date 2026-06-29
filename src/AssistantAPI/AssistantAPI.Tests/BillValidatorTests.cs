using AssistantAPI.Application.DTOs;
using AssistantAPI.Application.Validators;
using FluentValidation.TestHelper;

namespace AssistantAPI.Tests;

public class BillValidatorTests
{
    private readonly CreateBillRequestValidator _validator = new();

    [Fact]
    public void CreateBillRequest_ShouldFail_WhenNameIsEmpty()
    {
        var model = new CreateBillRequest("", 100, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), null);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateBillRequest_ShouldFail_WhenAmountIsZero()
    {
        var model = new CreateBillRequest("Light", 0, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), null);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void CreateBillRequest_ShouldFail_WhenDueDateIsInThePast()
    {
        var model = new CreateBillRequest("Light", 100, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)), null);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DueDate);
    }

    [Fact]
    public void CreateBillRequest_ShouldPass_WithValidData()
    {
        var model = new CreateBillRequest("Light", 100, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), null);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
