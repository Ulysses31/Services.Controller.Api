using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;
using Serilog.Core;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Services;

namespace Services.Controllers.API
{
  /// <summary>
  /// Shared class
  /// </summary>
  public class Shared
  {
    /// <summary>
    /// GetHostIpAddress function
    /// </summary>
    /// <returns>String</returns>
    public string GetHostIpAddress()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

      foreach (var item in ipHostInfo.AddressList)
      {
        if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
          IPAddress ipAddress = item;
          return ipAddress.ToString();
        }
      }

      return String.Empty;
    }

    /// <summary>
    /// Logging user activity
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="next">Task</param>
    /// <param name="_logger">Logger</param>
    /// <param name="requesterInfo">RequesterInfo</param>
    /// <param name="jsonOptions">JsonSerializerOptions</param>
    /// <returns>void</returns>
    public async Task LogUserActivity(
      HttpContext context,
      Func<Task> next,
      Logger _logger,
      RequesterInfo requesterInfo,
      JsonSerializerOptions jsonOptions
    )
    {
      if (context.Request.Path.StartsWithSegments("/swagger"))
      {
        await next.Invoke();
        return;
      }

      // Do work that can write to the Response.
      _logger.Information($"===> ⏰ Request started at: {DateTime.Now}");

      //****** Request body ********
      context.Request.EnableBuffering();
      using var reader = new StreamReader(
        context.Request.Body,
        Encoding.UTF8,
        leaveOpen: true
      );
      var reqBody = await reader.ReadToEndAsync();
      context.Request.Body.Position = 0;
      //***************************** 

      //****** Response body ********
      var orig = context.Response.Body;
      using var memoryStream = new MemoryStream();
      context.Response.Body = memoryStream;
      //***************************** 

      var acceptHeader = context.Request.Headers["Accept"].ToString();

      //if(acceptHeader.Equals("*/*"))
      //  context.Response.StatusCode = 415;

      if (acceptHeader.Contains("application/json"))
        context.Response.ContentType = "application/json";

      if (acceptHeader.Contains("application/xml"))
      {
        context.Response.ContentType = "application/xml";
        if (context.Request.Method.Equals("POST"))
        {
          context.Response.StatusCode = 201;
        }
      }

      await next.Invoke();

      //****** Response body ********
      memoryStream.Seek(0, SeekOrigin.Begin);
      var respBody = await new StreamReader(memoryStream).ReadToEndAsync();
      memoryStream.Seek(0, SeekOrigin.Begin);
      await memoryStream.CopyToAsync(orig);
      //***************************** 

      requesterInfo = new RequesterInfo()
      {
        RequestMethod = context.Request.Method,
        RequestPath = context.Request.Path,
        RequestBody = reqBody,
        RequestHeaders = JsonSerializer.Serialize(context.Request.Headers, new JsonSerializerOptions { WriteIndented = false }),
        ResponseHeaders = JsonSerializer.Serialize(context.Response.Headers, new JsonSerializerOptions { WriteIndented = false }),
        ResponseStatusCode = context.Response.StatusCode.ToString(),
        ResponseBody = respBody
      };

      // Log user activity to database
      // _logger.Information(JsonSerializer.Serialize(requesterInfo, jsonOptions));
      UserActivityLogDto userActivityLogDto = new UserActivityLogDto()
      {
        SourceName = requesterInfo.reqInfo.SourceName,
        OsVersion = requesterInfo.reqInfo.OsVersion,
        Host = requesterInfo.hostInfo.Hostname,
        Username = requesterInfo.hostInfo.Username,
        DomainName = requesterInfo.hostInfo.userDomainName,
        Address = requesterInfo.hostInfo.Addr,
        RequestMethod = requesterInfo.RequestMethod,
        RequestPath = requesterInfo.RequestPath,
        RequestBody = requesterInfo.RequestBody,
        RequestHeaders = requesterInfo.RequestHeaders,
        ResponseHeaders = requesterInfo.ResponseHeaders,
        ResponseStatusCode = requesterInfo.ResponseStatusCode,
        ResponseBody = requesterInfo.ResponseBody,
        RequestTime = DateTime.Now.ToString(),
        CreatedBy = "System"
      };

      using (var scope = context.RequestServices.CreateScope())
      {
        try
        {
          var _userActivityDbRepo = scope.ServiceProvider.GetRequiredService<UserActivityDbRepo>();
          await _userActivityDbRepo.CreateAsync(userActivityLogDto);
        }
        catch (System.Exception ex)
        {
          throw new Exception(ex.Message, ex.InnerException);
        }
      }

      _logger.Information($"===> ⏰ Request finished at: {DateTime.Now}");
    }


    /// <summary>
    /// GenerateHostedApiDoc function 
    /// </summary>
    /// <param name="openApiLocation">string</param>
    /// <param name="className">string</param>
    /// <param name="version">string</param>
    /// <param name="_logger">Logger</param>
    /// <returns>Task</returns>
    public async Task GenerateHostedApiDoc(
      string openApiLocation,
      string className,
      string version,
      ILogger<GenApiHostedService> _logger
    )
    {
      var document = await OpenApiDocument.FromUrlAsync(openApiLocation);

      #region CSharpController

      var settingsController = new CSharpControllerGeneratorSettings
      {
        ControllerBaseClass = "ControllerBase",
        GenerateClientClasses = true,
        GenerateClientInterfaces = true,
        CSharpGeneratorSettings = { Namespace = "Controllers" },
      };

      var generatorController = new CSharpControllerGenerator(document, settingsController);
      var generatedCodeController = generatorController.GenerateFile();
      var pathController = $"./ApiGen/{version}/{settingsController.CSharpGeneratorSettings.Namespace}/{className}_controller.cs";

      _logger.LogInformation($"===> 💻 Generated controller file path: {pathController}");

      var fileController = new FileInfo(pathController);
      fileController.Directory?.Create();
      await File.WriteAllTextAsync(fileController.FullName, generatedCodeController);

      #endregion CSharpController

      #region CSharpClient

      var settingsClient = new CSharpClientGeneratorSettings
      {
        UseBaseUrl = false,
        ClassName = className,
        GenerateClientInterfaces = true,
        CSharpGeneratorSettings = { Namespace = "HttpClients" },
      };

      var generatorClient = new CSharpClientGenerator(document, settingsClient);
      var generatedCodeClient = generatorClient.GenerateFile();
      var pathClient = $"./ApiGen/{version}/{settingsClient.CSharpGeneratorSettings.Namespace}/{settingsClient.ClassName}_client.cs";

      _logger.LogInformation($"===> 💻 Generated csharp-client file path: {pathClient}");

      var fileClient = new FileInfo(pathClient);
      fileClient.Directory?.Create();
      await File.WriteAllTextAsync(fileClient.FullName, generatedCodeClient);

      #endregion CSharpClient

      #region TypescriptClient

      var settingsTypescript = new TypeScriptClientGeneratorSettings
      {
        ClassName = className,
        Template = TypeScriptTemplate.Fetch,
        TypeScriptGeneratorSettings = { Namespace = "HttpClients" },
      };

      var generatorTypescript = new TypeScriptClientGenerator(document, settingsTypescript);
      var generatedCodeTypescript = generatorTypescript.GenerateFile();
      var pathTypescript = $"./ApiGen/{version}/{settingsTypescript.TypeScriptGeneratorSettings.Namespace}/{settingsTypescript.ClassName}.ts";

      _logger.LogInformation($"===> 💻 Generated typescript-client file path: {pathTypescript}");

      var fileTypescript = new FileInfo(pathTypescript);
      fileTypescript.Directory?.Create();
      await File.WriteAllTextAsync(fileTypescript.FullName, generatedCodeTypescript);

      #endregion TypescriptClient
    }

  }
}
