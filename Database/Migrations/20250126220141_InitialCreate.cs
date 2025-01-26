using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Controllers.API.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar", maxLength: 36, nullable: false, comment: "The unique identifier for the weather forecast."),
                    Date = table.Column<DateTime>(type: "date", nullable: false, comment: "The date of the weather forecast."),
                    TemperatureC = table.Column<int>(type: "int", nullable: false, comment: "The temperature in Celsius for the forecasted date."),
                    TemperatureF = table.Column<int>(type: "int", nullable: false, comment: "The temperature in Fahrenheit for the forecasted date, calculated from the temperature in Celsius."),
                    Summary = table.Column<string>(type: "varchar", nullable: false, comment: "A summary description of the weather (e.g., sunny, rainy)."),
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
                values: new object[] { "c3d61411-9123-4839-ad75-004507d26e85", "System", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sunny", 20, 0 });

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
                name: "WeatherForecast");
        }
    }
}
