using Microsoft.OpenApi.Models;

namespace DynamicAuth.Infrastructures.Swagger;

public static class SwaggerConfig
{
    public static void SwaggerGenInit(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
         {
             // Bearer token authentication
             OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
             {
                 Name = "Bearer",
                 BearerFormat = "JWT",
                 Scheme = "bearer",
                 Description = "請輸入token",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.Http,
             };
             c.AddSecurityDefinition("jwt_auth", securityDefinition);

             // Make sure swagger UI requires a Bearer token specified
             OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
             {
                 Reference = new OpenApiReference()
                 {
                     Id = "jwt_auth",
                     Type = ReferenceType.SecurityScheme
                 }
             };
             OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
              {
                    {securityScheme, new string[] { }},
              };
             c.AddSecurityRequirement(securityRequirements);
         });

    }
}
