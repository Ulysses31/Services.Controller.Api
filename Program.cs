
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using GenApi.Hosted.Service;
using Serilog;

namespace Services.Controllers.API;

public class Program
{
  public static void Main(string[] args)
  {
    RequesterInfo requesterInfo = new RequesterInfo();
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    IServiceCollection services = builder.Services;
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

    services.AddControllers();

    // Add services to the container.
    services.AddProblemDetails();

    // Add essential services to the dependency injection container
    services.AddAuthorization();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    // Cache generated OpenAPI document
    services.AddOutputCache(options =>
    {
      options.AddBasePolicy(policy =>
        policy.Expire(TimeSpan.FromMinutes(10)));
    });

    // Configures Swagger/OpenAPI for API documentation.
    services.CommonSwaggerSetup($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

    // Add hosted service to the dependency injection container
    if (envName.Equals("Development", StringComparison.Ordinal))
    {
      services.AddHostedService<GenApiHostedService>();
    }

    // ******************* APP ******************************************//
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseExceptionHandler("/error-development");
      app.UseCommonSwagger();
      app.UseOutputCache();
    }
    else
    {
      // Enables HTTPS redirection for secure communication in production.
      app.UseHttpsRedirection();
      app.UseExceptionHandler("/error");
    }

    app.UseStatusCodePages();

    app.UseHttpLogging();

    app.UseAuthorization();

    app.MapControllers();

    _logger.Information("===> Environment: {envName}", envName);
    _logger.Information("===> Host: {HostIpAddress}", requesterInfo.hostInfo.Addr);

    app.Run();
  }
}
