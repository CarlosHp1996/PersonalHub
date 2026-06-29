using AssistantAPI.Application.DTOs;
using AssistantAPI.Application.Services;
using AssistantAPI.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AssistantAPI.Api.Endpoints;

public static class BillEndpoints
{
    public static void MapBillEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/bills").WithTags("Bills");

        group.MapGet("/", async (IBillService svc, [FromQuery] string? status) => {
            var statusEnum = status != null ? Enum.Parse<BillStatus>(status, true) : (BillStatus?)null;
            return Results.Ok(await svc.GetAllAsync(statusEnum));
        });

        group.MapGet("/{id:guid}", async (IBillService svc, Guid id) => {
            var bill = await svc.GetByIdAsync(id);
            return bill is null ? Results.NotFound() : Results.Ok(bill);
        });

        group.MapPost("/", async (IBillService svc, IValidator<CreateBillRequest> validator,
            CreateBillRequest request) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());
            var bill = await svc.CreateAsync(request);
            return Results.Created($"/api/bills/{bill.Id}", bill);
        });

        group.MapPut("/{id:guid}", async (IBillService svc, IValidator<UpdateBillRequest> validator, Guid id, UpdateBillRequest request) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());
            var bill = await svc.UpdateAsync(id, request);
            return bill is null ? Results.NotFound() : Results.Ok(bill);
        });

        group.MapPatch("/{id:guid}/pay", async (IBillService svc, Guid id) => {
            var bill = await svc.MarkAsPaidAsync(id);
            return bill is null ? Results.NotFound() : Results.Ok(bill);
        });

        group.MapDelete("/{id:guid}", async (IBillService svc, Guid id) => {
            var deleted = await svc.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
