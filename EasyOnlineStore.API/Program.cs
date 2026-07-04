using System.ComponentModel.DataAnnotations;
using System.Text;
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
using EasyOnlineStore.Domain.Models.Users;
using EasyOnlineStore.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// base services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// mapper
builder.Services.AddAutoMapper(
    typeof(UserProfile).Assembly,
    typeof(ProductProfile).Assembly,
    typeof(CartProfile).Assembly,
    typeof(OrderProfile).Assembly,
    typeof(WarehouseProfile).Assembly,
    typeof(CategoryProfile).Assembly);


// data base contextt
builder.Services.AddDbContext<EasyOnlineStoreDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(EasyOnlineStoreDbContext)));
    });


// microsoft user identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EasyOnlineStoreDbContext>()
    .AddDefaultTokenProviders();

// jwt setting
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtOptions>() 
                  ?? throw new Exception("JwtSettings not configured");

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<JwtProvider>(); 

// authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

// registration repositories
builder.Services.AddScoped<IUserRepository, UserRepository>(
    provider => new UserRepository(provider.GetRequiredService<EasyOnlineStoreDbContext>()));

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

// registration services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


// enable custom exceptions
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// build app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// global custom esceptions
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
                NotFoundException => StatusCodes.Status404NotFound,
                InsufficientStockException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                ConflictException => StatusCodes.Status409Conflict,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            },
            Instance = exceptionHandlerPathFeature?.Path,
            Type = exception?.GetType().FullName
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

// routing
app.UseRouting();
app.UseHttpsRedirection();

// authentication
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
