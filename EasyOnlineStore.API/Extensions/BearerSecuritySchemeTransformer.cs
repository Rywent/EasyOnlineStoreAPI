using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;


namespace EasyOnlineStore.API.Extensions;

internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // 1. Английский текст подсказки для токена
        var bearerScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token. Example: eyJhbGciOi..."
        };
        
        document.Components ??= new OpenApiComponents();
        if (document.Components.SecuritySchemes == null)
        {
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
        }
        
        document.Components.SecuritySchemes["Bearer"] = bearerScheme;
        
        var securityRequirement = new OpenApiSecurityRequirement
        {
            { 
                new OpenApiSecuritySchemeReference("Bearer", document), 
                new List<string>() 
            }
        };
        
        var provider = context.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
        
        var apiDescriptions = provider.ApiDescriptionGroups.Items
            .SelectMany(g => g.Items)
            .ToList();

        if (document.Paths != null)
        {
            foreach (var pathKvp in document.Paths)
            {
                var pathKey = pathKvp.Key;
                var pathValue = pathKvp.Value;

                if (pathValue.Operations == null) continue;

                foreach (var operationKvp in pathValue.Operations)
                {
                    var httpMethod = operationKvp.Key.ToString();
                    var operation = operationKvp.Value;
                    
                    var apiDescription = apiDescriptions.FirstOrDefault(d => 
                    {
                        var relativePath = d.RelativePath?.TrimEnd('/');
                        var formattedPath = relativePath?.StartsWith('/') == true ? relativePath : $"/{relativePath}";
                        
                        return string.Equals(formattedPath, pathKey.TrimEnd('/'), StringComparison.OrdinalIgnoreCase) && 
                               string.Equals(d.HttpMethod, httpMethod, StringComparison.OrdinalIgnoreCase);
                    });

                    if (apiDescription != null)
                    {
                        var endpointMetadata = apiDescription.ActionDescriptor.EndpointMetadata;
                        
                        var hasAuthorize = endpointMetadata.Any(m => m is IAuthorizeData);
                        var hasAllowAnonymous = endpointMetadata.Any(m => m is IAllowAnonymous);
                        
                        if (hasAuthorize && !hasAllowAnonymous)
                        {
                            operation.Security ??= new List<OpenApiSecurityRequirement>();
                            operation.Security.Add(securityRequirement);
                        }
                    }
                    else
                    {
                        if (!pathKey.Contains("/Auth/", StringComparison.OrdinalIgnoreCase))
                        {
                            operation.Security ??= new List<OpenApiSecurityRequirement>();
                            operation.Security.Add(securityRequirement);
                        }
                    }
                }
            }
        }

        return Task.CompletedTask;
    }
}