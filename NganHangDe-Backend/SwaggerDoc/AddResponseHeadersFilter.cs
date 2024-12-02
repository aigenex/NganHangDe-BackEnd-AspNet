using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddResponseHeadersFilter : IOperationFilter
{
    // use this to add response headers
    // add padlock icon to the endpoint that has authorize attribute
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        // use this to check if the method has the attribute
        if (context.MethodInfo.DeclaringType is null)
            return;

        // check for authorize attribute
        var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                    || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        
        

        if (hasAuthorize)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            // operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            var jwtBearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [jwtBearerScheme] = Array.Empty<string>()
                }
            };
        }

        // check authorize has policy, roles
        var authorizeAttributes = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().ToList();

        if (authorizeAttributes.Any())
        {
            var authorizeAttribute = authorizeAttributes.First();
            if (!string.IsNullOrEmpty(authorizeAttribute.Policy))
            {
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }
            if (!string.IsNullOrEmpty(authorizeAttribute.Roles))
            {
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }
        }
    }
}