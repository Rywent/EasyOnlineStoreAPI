using EasyOnlineStore.API.Extensions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Mapping;
using EasyOnlineStore.Application.Services;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Persistence;
using EasyOnlineStore.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyOnlineStore.Domain.Models.Users;
using EasyOnlineStore.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// base services
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});



// mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<ProductProfile>();
    cfg.AddProfile<CartProfile>();
    cfg.AddProfile<OrderProfile>();
    cfg.AddProfile<WarehouseProfile>();
    cfg.AddProfile<CategoryProfile>();
});


// data base context
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
builder.Services.AddApiAuthentication(jwtSettings);
builder.Services.AddApiAuthorization();

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


// disable automatic ModelState validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// build app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
}

// global custom exceptions
app.UseGlobalExceptionHandler();

// routing
app.UseRouting();
app.UseHttpsRedirection();

// authentication
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
