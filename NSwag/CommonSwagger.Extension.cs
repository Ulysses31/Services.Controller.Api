using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;

namespace Services.Controllers.API
{
  /// <summary>
  /// Provides extension methods for setting up Swagger and API versioning in the application.
  /// </summary>
  public static class CommonSwaggerExtension
  {
    /// <summary>
    /// Configures Swagger and API versioning for the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="xmlDocumentFile">The path to the XML documentation file for Swagger.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection CommonSwaggerSetup(
        this IServiceCollection services,
        string? xmlDocumentFile
    )
    {
      // Add authentication using JWT Bearer tokens.
      //TODO:  services.AddAuthentication().AddJwtBearer();

      // Add authorization with custom policies.
      //TODO: services.AddAuthorizationBuilder()
      //   .AddPolicy("admin_greetings", policy =>
      //     policy
      //       .RequireRole("admin")
      //       .RequireClaim("scope", "greetings_api"));

      // Add API endpoint explorer for Swagger.
      //   services.AddEndpointsApiExplorer();
      services.AddOpenApiDocument(options =>
      {
        // provider?.ApiVersionDescriptions.ToList().ForEach(description =>
        // {
        //   options.PostProcess = document =>
        //   {
        //     document.Info.Version = description.ApiVersion.ToString();
        //     document.Info.Title = "API";
        //     document.Info.Description = "A simple API for testing.";
        //     document.Info.TermsOfService = "None";
        //     document.Info.Contact = new NSwag.OpenApiContact
        //     {
        //       Name = "API Support",
        //       Email = string.Empty,
        //       Url = "https://www.example.com/support"
        //     };
        //     document.Info.License = new NSwag.OpenApiLicense
        //     {
        //       Name = "Use under LICX",
        //       Url = "https://www.example.com/license"
        //     };
        //   };
        // });
      });

      // Configure API versioning.
      services.AddApiVersioning(
          options =>
          {
            // Enable reporting of supported and deprecated API versions in response headers.
            options.ReportApiVersions = true;
            // Configure API version readers (e.g., URL segment, headers, media type).
            options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version")
                  );
            // Define a sunset policy for API version 0.9.
            options.Policies.Sunset(0.9)
                      .Effective(DateTimeOffset.Now.AddDays(60))
                      .Link("policy.html")
                          .Title("Versioning Policy")
                          .Type("text/html");
          })
          .AddApiExplorer(
              options =>
              {
                // Set the format for grouping API versions in Swagger.
                options.GroupNameFormat = "'v'VVV";
                // Enable substitution of API versions in URL templates.
                options.SubstituteApiVersionInUrl = true;
              });

      // Configure Swagger options using a custom options provider.
      // services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

      // Add and configure Swagger generator.
      //   services.AddSwaggerGen(options =>
      //   {
      // Apply default values to Swagger operations.
      //     options.OperationFilter<SwaggerDefaultValues>();

      // Include XML comments for API documentation if a file path is provided.
      //     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDocumentFile!));

      // Add security definitions for Bearer tokens.
      //TODO: options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      // {
      //   Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
      //   Name = "Authorization",
      //   In = ParameterLocation.Header,
      //   Type = SecuritySchemeType.ApiKey
      // });

      // Require security schemes for all API endpoints.
      //TODO: options.AddSecurityRequirement(
      //   new OpenApiSecurityRequirement {
      //       {
      //           new OpenApiSecurityScheme
      //           {
      //               Reference = new OpenApiReference
      //               {
      //                 Type = ReferenceType.SecurityScheme,
      //                 Id = "Bearer"
      //               }
      //           },
      //           Array.Empty<string>()
      //       }
      //   });
      //  });

      return services;
    }

    /// <summary>
    /// Configures Swagger middleware for the application.
    /// </summary>
    /// <param name="app">The web application to configure.</param>
    /// <returns>The modified <see cref="WebApplication"/>.</returns>
    public static WebApplication UseCommonSwagger(
        this WebApplication app
    )
    {
      var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

      // Enable Swagger middleware.
      //app.UseSwagger();
      app.UseOpenApi(options =>
      {
        foreach (var description in app.DescribeApiVersions())
        {
          options.PostProcess = (document, request) =>
          {
            document.Info.Version = description.ApiVersion.ToString();
            document.Info.Title = "API";
            document.Info.Description = "A simple API for testing.";
            document.Info.TermsOfService = "None";
            document.Info.Contact = new NSwag.OpenApiContact
            {
              Name = "API Support",
              Email = string.Empty,
              Url = "https://www.example.com/support"
            };
            document.Info.License = new NSwag.OpenApiLicense
            {
              Name = "Use under LICX",
              Url = "https://www.example.com/license"
            };
          };
        }
      });

      // Configure Swagger UI.
      app.UseSwaggerUi(
          options =>
          {
            var descriptions = app.DescribeApiVersions();
            // Add Swagger endpoints for each API version.
            foreach (var description in descriptions)
            {
              var url = $"/swagger/{description.GroupName}/swagger.json";
              var name = description.GroupName.ToUpperInvariant();
              options.SwaggerRoutes.Add(new SwaggerUiRoute(name, url));
            }
          }
      );

      return app;
    }
  }
}
