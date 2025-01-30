using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Controllers.API.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "The unique identifier for the log entry.")
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceName = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The source name of the request."),
                    OsVersion = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The operating system version of the client making the request."),
                    Host = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The hostname of the client."),
                    Username = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The username of the user making the request."),
                    DomainName = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The domain name associated with the request."),
                    Address = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The IP address of the client making the request."),
                    RequestMethod = table.Column<string>(type: "text", maxLength: 10, nullable: true, comment: "The HTTP method used in the request (e.g., GET, POST)."),
                    RequestPath = table.Column<string>(type: "text", maxLength: 100, nullable: true, comment: "The path of the requested resource."),
                    RequestTime = table.Column<string>(type: "datetime", nullable: true, comment: "The timestamp of when the request was made."),
                    RequestBody = table.Column<string>(type: "text", nullable: true, comment: "The body content of the request."),
                    RequestHeaders = table.Column<string>(type: "text", nullable: true, comment: "The headers included in the request."),
                    ResponseHeaders = table.Column<string>(type: "text", nullable: true, comment: "The headers included in the response."),
                    ResponseStatusCode = table.Column<string>(type: "text", maxLength: 10, nullable: true, comment: "The HTTP status code of the response."),
                    ResponseBody = table.Column<string>(type: "text", nullable: true, comment: "The body content of the response."),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "Who created the record."),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivity_UserActivityId", x => x.Id);
                },
                comment: "A log entry for user activity within the system.");

            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", maxLength: 36, nullable: false, comment: "The unique identifier for the weather forecast."),
                    Date = table.Column<string>(type: "date", nullable: false, comment: "The date of the weather forecast."),
                    TemperatureC = table.Column<int>(type: "integer", nullable: false, comment: "The temperature in Celsius for the forecasted date."),
                    TemperatureF = table.Column<int>(type: "integer", nullable: false, comment: "The temperature in Fahrenheit for the forecasted date, calculated from the temperature in Celsius."),
                    Summary = table.Column<string>(type: "text", nullable: false, comment: "A summary description of the weather (e.g., sunny, rainy)."),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "Who created the record."),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast_WeatherForecastId", x => x.Id);
                },
                comment: "A weather forecast for a specific date.");

            migrationBuilder.InsertData(
                table: "WeatherForecast",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Date", "ModifiedDate", "Summary", "TemperatureC", "TemperatureF" },
                values: new object[,]
                {
                    { "1130f076-1d75-4977-8a50-323a4ecf8f4e", "System", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-01-11", null, "Hot", 35, 94 },
                    { "2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04", "System", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-01-20", null, "Warm", 20, 67 },
                    { "38b7942a-8a8f-4a34-9744-e4dea6eaed78", "System", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-01-04", null, "Hot", 25, 76 },
                    { "3db3a34a-9dcf-42e6-977f-d6bbb2329f16", "System", new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-01-07", null, "Cool", 15, 58 },
                    { "76d5e039-63b3-4c7f-bb8d-0847f729dcde", "System", new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-01-09", null, "Cold", 5, 40 }
                });

            migrationBuilder.CreateIndex(
                name: "AK_UserActivity_UserActivityId",
                table: "UserActivity",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_WeatherForecast_WeatherForecastId",
                table: "WeatherForecast",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActivity");

            migrationBuilder.DropTable(
                name: "WeatherForecast");
        }
    }
}
