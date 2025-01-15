
namespace Services.Controllers.API;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers(options => {
      //options.RespectBrowserAcceptHeader = true;
    })
    .AddXmlDataContractSerializerFormatters();

    builder.Services.AddProblemDetails();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    // Cache generated OpenAPI document
    builder.Services.AddOutputCache(options =>
    {
      options.AddBasePolicy(policy =>
        policy.Expire(TimeSpan.FromMinutes(10)));
    });

    builder.Services.AddOpenApi(options =>
    {
      options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    });


    //*********** APP *******************************************************//

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseExceptionHandler("/error-development");

      app.UseOutputCache();
      app.MapOpenApi("/openapi/{documentName}/swagger.json").CacheOutput();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("/openapi/v1/swagger.json", "v1");
        // options.SwaggerEndpoint("/openapi/v2/swagger.json", "v2");
      });
    }
    else
    {
      app.UseExceptionHandler("/error");
    }

    app.UseStatusCodePages();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
  }
}
