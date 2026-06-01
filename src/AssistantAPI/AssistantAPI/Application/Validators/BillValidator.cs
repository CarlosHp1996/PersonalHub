using AssistantAPI.Application.DTOs;
using FluentValidation;

namespace AssistantAPI.Application.Validators;

public class CreateBillRequestValidator : AbstractValidator<CreateBillRequest>
{
    public CreateBillRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
    }
}

public class UpdateBillRequestValidator : AbstractValidator<UpdateBillRequest>
{
    public UpdateBillRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
