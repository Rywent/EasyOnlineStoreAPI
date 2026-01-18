using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Mapping;
using EasyOnlineStore.Application.Services;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Persistence;
using EasyOnlineStore.Persistence.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly,
    typeof(CartProfile).Assembly,
    typeof(OrderProfile).Assembly,
    typeof(WarehouseProfile).Assembly,
    typeof(CategoryProfile).Assembly);


builder.Services.AddDbContext<EasyOnlineStoreDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(EasyOnlineStoreDbContext)));
    });

builder.Services.AddScoped<IProductRepository, ProductRepository>(
    provider => new ProductRepository(provider.GetRequiredService<EasyOnlineStoreDbContext>()));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(
    provider => new CategoryRepository(provider.GetRequiredService<EasyOnlineStoreDbContext>()));

builder.Services.AddScoped<ICartRepository, CartRepository>(
    provider => new CartRepository(provider.GetRequiredService<EasyOnlineStoreDbContext>()));

builder.Services.AddScoped<IOrderRepository, OrderRepository>(
    provider => new OrderRepository(provider.GetRequiredService<EasyOnlineStoreDbContext>()));

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>(
    provider => new WarehouseRepository(provider.GetRequiredService<EasyOnlineStoreDbContext>()));


builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var problemDetails = new ProblemDetails
        {
            Title = exception?.GetType().Name ?? "Internal Server Error",
            Detail = exception?.Message,
            Status = exception switch
            {
                NotFoundException => 404,
                InsufficientStockException => 400,
                _ => 500
            },
            Instance = exceptionHandlerPathFeature?.Path,
            Type = exception?.GetType().FullName
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
