using MediatR;
using AutoMapper;
using SalesApi.Application.Mappings;
using SalesApi.Application.Commands;
using SalesApi.Infrastructure.Repositories;
using SalesApi.Application.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(DomainToDtoMappingProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateSaleCommandHandler>());
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ISaleRepository, SaleRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (InvalidSaleException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            type = "BadRequest",
            error = "Invalid Sell",
            detail = ex.Message
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            type = "InternalServerError",
            error = "An unexpected error occurred.",
            detail = ex.Message
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
});

app.UseAuthorization();
app.MapControllers();
app.Run();