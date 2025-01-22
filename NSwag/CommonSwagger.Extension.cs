using System.Globalization;
using System.Net;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using NSwag;
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
    public static IServiceCollection CommonSwaggerSetup<T>(
        this IServiceCollection services,
        string? xmlDocumentFile
    ) where T : new()
    {
      // Add authentication using JWT Bearer tokens.
      //TODO:  services.AddAuthentication().AddJwtBearer();

      // Add authorization with custom policies.
      //TODO: services.AddAuthorizationBuilder()
      //   .AddPolicy("admin_greetings", policy =>
      //     policy
      //       .RequireRole("admin")
      //       .RequireClaim("scope", "greetings_api"));

      // Configure API versioning.
      services.AddApiVersioning(
          options =>
          {
            // // Enable reporting of supported and deprecated API versions in response headers.
            options.ReportApiVersions = true;
            // // Configure API version readers (e.g., URL segment, headers, media type).
            options.ApiVersionReader = ApiVersionReader.Combine(
                    // new QueryStringApiVersionReader("api-version"),
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version")
                  );
            // // Define a sunset policy for API version 0.9.
            // options.Policies.Sunset(0.9)
            //           .Effective(DateTimeOffset.Now.AddDays(60))
            //           .Link("policy.html")
            //               .Title("Versioning Policy")
            //               .Type("text/html");
          })
           .AddVersionedApiExplorer(
              options =>
              {
                // Set the format for grouping API versions in Swagger.
                options.GroupNameFormat = "'v'VVV";
                // Enable substitution of API versions in URL templates.
                options.SubstituteApiVersionInUrl = true;
              });

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


      // Add API endpoint explorer for Swagger.
      var apiVersionDescriptionProvider
        = services.BuildServiceProvider()
                  .GetRequiredService<IApiVersionDescriptionProvider>();
      var apiConfiguration
        = services.BuildServiceProvider()
                  .GetRequiredService<IConfiguration>();
      apiConfiguration.GetSection(CommonSwaggerOptions.MySwagger)
                      .Bind(new CommonSwaggerOptions());

      var xmlFile = $"{System.AppDomain.CurrentDomain.FriendlyName}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

      services.AddEndpointsApiExplorer();

      foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
      {
        services.AddOpenApiDocument((options) =>
        {
          options.DocumentName = description.GroupName;
          options.ApiGroupNames = new[] { description.GroupName };
          options.UseControllerSummaryAsTagDescription = true;
          options.PostProcess = document =>
          {
            document.Info.Version = description.ApiVersion.ToString();
            document.Info.Title = CommonSwaggerOptions.Title;
            document.Info.Description = CommonSwaggerOptions.Description;
            document.Info.TermsOfService = CommonSwaggerOptions.TermsOfService;
            document.Info.Contact = new NSwag.OpenApiContact
            {
              Name = CommonSwaggerOptions.Contact?.Name,
              Email = CommonSwaggerOptions.Contact?.Email,
              Url = CommonSwaggerOptions.Contact?.Url
            };
            document.Info.License = new NSwag.OpenApiLicense
            {
              Name = CommonSwaggerOptions.License?.Name,
              Url = CommonSwaggerOptions.License?.Url
            };

            // Add supported media types globally
            string exampleDto = SerializeToXml(new T());

            string prb = @"
              <?xml version=""1.0"" encoding=""utf-8""?>
              <problem>
                <type>ProblemDetails</type>
                <title>Internal Server Error</title>
                <status>500</status>
                <detail>An unexpected error occurred.</detail>
                <instance>https://www.example.com/error/500</instance>
              </problem>
            ";

            foreach (var path in document.Paths.Values)
            {
              foreach (var operation in path.Values)
              {

                if (operation.Responses.ContainsKey("200"))
                {
                  operation.Responses["200"].Content.Add("application/xml", new OpenApiMediaType()
                  {
                    Example = WebUtility.HtmlDecode(exampleDto)
                  });
                }

                if (operation.Responses.ContainsKey("201"))
                {
                  operation.Responses["201"].Content.Add("application/xml", new OpenApiMediaType()
                  {
                    Example = WebUtility.HtmlDecode(exampleDto)
                  });
                }

                if (operation.Responses.ContainsKey("204"))
                {
                  operation.Responses["204"].Content.Remove("application/json");
                  operation.Responses["204"].Content.Remove("application/xml");
                }

                if (operation.Responses.ContainsKey("400"))
                {
                  operation.Responses["400"].Content.Add("application/xml", new OpenApiMediaType()
                  {
                    Example = WebUtility.HtmlDecode(prb)
                  });
                }

                if (operation.Responses.ContainsKey("404"))
                {
                  operation.Responses["404"].Content.Add("application/xml", new OpenApiMediaType()
                  {
                    Example = WebUtility.HtmlDecode(prb)
                  });
                }

                if (operation.Responses.ContainsKey("500"))
                {
                  operation.Responses["500"].Content.Add("application/xml", new OpenApiMediaType()
                  {
                    Example = WebUtility.HtmlDecode(prb)
                  });
                }

                if (operation.RequestBody != null)
                {
                  if (!operation.RequestBody.Content.ContainsKey("application/xml"))
                    operation.RequestBody.Content["application/xml"] = new OpenApiMediaType()
                    {
                      Example = WebUtility.HtmlDecode(exampleDto)
                    };
                }

                // Iterate through responses
                foreach (var response in operation.Responses.Values)
                {
                  if (response.Content != null)
                  {
                    if (operation.Responses.ContainsKey("200")
                      && operation.Responses.ContainsKey("201")
                      && operation.Responses.ContainsKey("400")
                      && operation.Responses.ContainsKey("404")
                      && operation.Responses.ContainsKey("500"))
                    {
                      // Add "application/xml" to Response.Content if not already present
                      if (!response.Content.ContainsKey("application/xml"))
                        response.Content.Add("application/xml", new OpenApiMediaType());
                    }
                  }
                }
              }
            }
          };
        });
      }

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
      app.UseOpenApi(options => { });

      // Configure Swagger UI.
      app.UseSwaggerUi(
          options =>
          {
            var descriptions = apiVersionDescriptionProvider.ApiVersionDescriptions;
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


    // Helper method to serialize a DTO to XML
    private static string SerializeToXml<T>(T obj)
    {
      var xmlSerializer = new XmlSerializer(typeof(T));
      using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
      {
        xmlSerializer.Serialize(stringWriter, obj);
        return WebUtility.HtmlDecode(stringWriter.ToString());
      }
    }


  }
}
