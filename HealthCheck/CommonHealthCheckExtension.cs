using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Services.Controllers.API.HealthCheck
{
  /// <summary>
  /// CommonHealthCheckExtension class
  /// </summary>
  public static class CommonHealthCheckExtension
  {
    /// <summary>
    /// CommonHealthCheckSetup function
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="dbConnectionString">string</param>
    /// <param name="apiEndpoint">string</param>
    /// <param name="dbTypeEnum">string</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection CommonHealthCheckSetup<T>(
      this IServiceCollection services,
      string apiEndpoint,
      string dbTypeEnum,
      string dbConnectionString
    ) where T : DbContext
    {
      var configuration = services.BuildServiceProvider()
                                  .GetRequiredService<IConfiguration>();

      // HTTP Clients
      services.AddHttpClient("api-health-check", options =>
      {
        // options.BaseAddress = new Uri("https://localhost:7000/api/orders/1");
        options.BaseAddress = new Uri(apiEndpoint);
        options.DefaultRequestHeaders.Add("x-api-version", "1.0");
      });

      switch (dbTypeEnum)
      {
        case "MsSql":
          services.AddHealthChecks()
              .AddSqlServer(dbConnectionString)
              .AddDbContextCheck<T>()
              .AddCheck<ApiHealthChecks>($"API {apiEndpoint}");
          break;
        case "MySql":
          break;
        case "SqLite":
          services.AddHealthChecks()
              .AddSqlite(dbConnectionString)
              .AddDbContextCheck<T>()
              .AddCheck<ApiHealthChecks>($"API {apiEndpoint}");
          break;
        case "MongoDb":
          break;
      }

      services.AddHealthChecksUI(options =>
      {
        options.SetEvaluationTimeInSeconds(60);       //Sets the time interval in which HealthCheck will be triggered
        options.MaximumHistoryEntriesPerEndpoint(60); //Sets the maximum number of records displayed in history
        options.SetApiMaxActiveRequests(1);
        options.AddHealthCheckEndpoint("Health Checks API", "/health"); //Sets the Health Check endpoint
      }).AddInMemoryStorage(); //Here is the memory bank configuration

      return services;
    }

    /// <summary>
    /// CommonHealthCheckUseSetup function
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <returns>WebApplication</returns>
    public static WebApplication CommonHealthCheckUseSetup(
        this WebApplication app
    )
    {
      //Sets Health Check dashboard options
      app.MapHealthChecks("/health", new HealthCheckOptions
      {
        Predicate = p => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      });

      //Sets the Health Check dashboard configuration
      app.MapHealthChecksUI(options =>
      {
        options.UIPath = "/dashboard";
        options.AsideMenuOpened = false;
      });

      return app;
    }
  }
}
