using ECommerceApi.API.Middleware;
using ECommerceApi.Application;
using ECommerceApi.Infrastructure;
using ECommerceApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "ECommerce API",
        Version = "v1",
        Description = "Multi-localization e-commerce API — DDD, CQRS, MediatR, EF Core & Redis."
    });
    // Fix: resolve conflicts by using full type name
    options.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Swagger FIRST before any custom middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
    c.RoutePrefix = "swagger";
});

// Validation middleware after Swagger
app.UseMiddleware<ValidationExceptionMiddleware>();

// Apply DB schema on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
