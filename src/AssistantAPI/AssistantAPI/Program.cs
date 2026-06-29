using AssistantAPI.Api.Endpoints;
using AssistantAPI.Application.Services;
using AssistantAPI.Application.Validators;
using AssistantAPI.Domain.Interfaces;
using AssistantAPI.Infrastructure.Data;
using AssistantAPI.Infrastructure.Data.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// DI
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBillRequestValidator>();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.Map("/error", () => Results.Problem());

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
app.MapBillEndpoints();

// Auto-migrate on startup (dev only — use proper migration strategy in prod)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
