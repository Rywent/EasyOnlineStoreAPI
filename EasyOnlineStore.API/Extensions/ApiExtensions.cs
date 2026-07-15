using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.RateLimiting;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace EasyOnlineStore.API.Extensions;

public static class ApiExtensions
{
    public static void AddApiAuthentication(this IServiceCollection services, JwtOptions jwtSettings)
    {
        services.AddAuthentication(options =>
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
    }

    public static void AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            
            options.AddPolicy("AdminPolicy", policy => 
                policy.RequireRole(RoleNames.Admin));
            
            options.AddPolicy("SellerPolicy", policy => 
                policy.RequireRole(RoleNames.Seller));
             
            options.AddPolicy("CustomerPolicy", policy => 
                policy.RequireRole(RoleNames.Customer));
            
            options.AddPolicy("DeveloperPolicy", policy => 
                policy.RequireRole(RoleNames.Developer));

            
        });
    }
    
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = exception.GetType().Name,
                    Detail = exception.Message,
                    Status = exception switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        InsufficientStockException => StatusCodes.Status400BadRequest,
                        ValidationException => StatusCodes.Status400BadRequest,
                        ConflictException => StatusCodes.Status409Conflict,
                        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                        _ => StatusCodes.Status500InternalServerError
                    },
                    Instance = context.Request.Path,
                    Type = exception.GetType().FullName
                };
                
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                if (problemDetails.Status >= 500)
                {
                    logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
                }
                else
                {
                    logger.LogWarning("Business error handled: {Message}", exception.Message);
                }
                
                context.Response.StatusCode = problemDetails.Status.Value;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        });
    }
        
    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ipAddress,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 150,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 2
                    });
            });
            
            options.AddPolicy("auth", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(5),
                        QueueLimit = 0
                    }));
        });
    }
    
}