using System.Diagnostics;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Services.Controllers.API.Configuration;
using Services.Controllers.API.Database.Contexts;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Mapping;
using Services.Controllers.API.RateLimit;
using Services.Controllers.API.Services;
using Services.Controllers.API.Validator;

namespace Services.Controllers.API;

public class Program
{
  public static void Main(string[] args)
  {
    RequesterInfo requesterInfo = new RequesterInfo();
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    IServiceCollection services = builder.Services;
    ConfigurationManager configuration = builder.Configuration;
    configuration.GetSection(CommonAppOptions.AppOptions)
                 .Bind(new CommonAppOptions());
    string envName = builder.Environment.EnvironmentName;


    // JSON Options  
    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
      WriteIndented = true,
      Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    // Logging setup
    #region Logger
    // Configures HTTP logging and Serilog for structured logging.
    services.AddHttpLogging(options =>
    {
      options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
      options.ResponseHeaders.Add("controller-api");
      options.MediaTypeOptions.AddText("application/json");
    });

    // Configure Serilog for structured logging with enriched properties
    var _logger = new LoggerConfiguration()
        .Enrich.WithProperty("Source", requesterInfo.reqInfo.SourceName)
        .Enrich.WithProperty("OSVersion", requesterInfo.reqInfo.OsVersion)
        .Enrich.WithProperty("ServerName", requesterInfo.hostInfo.Hostname)
        .Enrich.WithProperty("UserName", requesterInfo.hostInfo.Username)
        .Enrich.WithProperty("UserDomainName", requesterInfo.hostInfo.userDomainName)
        // Uncomment and implement to enrich logs with additional properties
        .Enrich.WithProperty("Address", requesterInfo.hostInfo.Addr)
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    // Add Serilog to the application's logging providers
    builder.Logging.AddSerilog(_logger);

    // Enable Serilog self-logging in development mode for debugging
    if (envName.Equals("Development", StringComparison.Ordinal))
    {
      Serilog.Debugging.SelfLog.Enable(Console.Error);
    }
    #endregion Logger

    services.AddControllers((options) =>
    {
      options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status429TooManyRequests));
      // options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
      // options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
      // options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status403Forbidden));
      // options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound));
      // options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
      options.InputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerInputFormatter(options));
      options.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter());
      options.ReturnHttpNotAcceptable = true;
    })
      .AddXmlSerializerFormatters()
      .AddXmlDataContractSerializerFormatters()
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
      });

    // Add services to the container.
    services.AddProblemDetails();

    // Add essential services to the dependency injection container
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.RequireHttpsMetadata = false;
      });
    services.AddAuthorization();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    // Cache generated OpenAPI document
    // services.AddOutputCache(options =>
    // {
    //   options.AddBasePolicy(policy =>
    //     policy.Expire(TimeSpan.FromMinutes(10)));
    // });

    // Configures Swagger/OpenAPI for API documentation.
    services.CommonSwaggerSetup<WeatherForecastDto>(
      $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
    );

    // Add hosted service to the dependency injection container
    if (envName.Equals("Development", StringComparison.Ordinal))
    {
      if (CommonAppOptions.EnableGenApiClient)
      {
        services.AddHostedService<GenApiHostedService>();
      }
    }

    // Rate Limiting
    if (CommonAppOptions.EnableRateLimiting)
    {
      services.CommonRateLimitSetup();
    }

    // Health Checks
    // if (CommonAppOptions.EnableHealthCheck) {
    //   services.CommonHealthCheckSetup<AuthorDbContextV1>(
    //       $"https://publications.io:7000/api/v1/publications/author",
    //       DbTypeEnum.MsSql,
    //       $"Server={server},{port};Database={database};User={user};Password={password};TrustServerCertificate=True"
    //   );
    // }

    // Mapping
    services.AddAutoMapper(typeof(MappingProfile));

    // Add FluentValidation to the dependency injection container
    services.AddScoped<IValidator<WeatherForecastDto>, DtoValidator>();

    // Database Context
    services.AddDbContext<ServicesDbContext>(options =>
    {
      // _ = options.UseSqlServer(
      //     $"Server={server},{port};Database={database};User={user};Password={password};Encrypt=Optional;TrustServerCertificate=True"
      // )
      options.UseSqlite(
        builder.Configuration["ConnectionStrings:SqlLiteConnection"],
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
      )
      .LogTo(
          message => Console.WriteLine(message),
          envName == "Development" ? LogLevel.Trace : LogLevel.Error,
          DbContextLoggerOptions.DefaultWithUtcTime
      )
      .LogTo(
          message => Debug.WriteLine(message),
          envName == "Development" ? LogLevel.Trace : LogLevel.Error,
          DbContextLoggerOptions.DefaultWithUtcTime
      )
       .EnableDetailedErrors(envName.Equals("Development", StringComparison.Ordinal))
       .EnableSensitiveDataLogging(envName.Equals("Development", StringComparison.Ordinal))
       .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
       .UseLazyLoadingProxies(o => {});
       // .UseAsyncSeeding(
       //    async (context, _, token) =>
       //    {
       //      await context.Database.EnsureCreatedAsync(token);
       //    }
       // );
    });
    
    // database repo services
    services.AddScoped<ServicesApiDbRepo>();
    services.AddScoped<UserActivityDbRepo>();

    // ******************* APP ******************************************//
    var app = builder.Build();

    // Middleware to log the start and end of each request
    app.Use(async (context, next) =>
    {
      await Task.Run(async () =>
      {
        await new Shared().LogUserActivity(
        context,
        next,
        _logger,
        requesterInfo,
        jsonOptions
      );
      });
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseExceptionHandler("/error-development");
      app.UseCommonSwagger();

      if (CommonAppOptions.EnableApiCache)
      {
        // app.UseOutputCache();
      }
    }
    else
    {
      // Enables HTTPS redirection for secure communication in production.
      app.UseHttpsRedirection();
      app.UseExceptionHandler("/error");
    }

    app.UseStatusCodePages();

    app.UseHttpLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    if (CommonAppOptions.EnableRateLimiting)
    {
      app.UseRateLimiter();
    }

    _logger.Information("===> Environment: {envName}", envName);
    _logger.Information("===> Host: {HostIpAddress}", requesterInfo.hostInfo.Addr);

    app.Run();

  }
}
