﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.Controllers.API.Database.Contexts;

#nullable disable

namespace Services.Controllers.API.Database.Migrations
{
    [DbContext(typeof(ServicesDbContext))]
    [Migration("20250202215112_InitialSetup")]
    partial class InitialSetup
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("Services.Controllers.API.Database.Models.UserActivityLogDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id")
                        .HasComment("The unique identifier for the log entry.");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Address")
                        .HasComment("The IP address of the client making the request.");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasComment("Who created the record.");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<string>("DomainName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("DomainName")
                        .HasComment("The domain name associated with the request.");

                    b.Property<string>("Host")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Host")
                        .HasComment("The hostname of the client.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.Property<string>("OsVersion")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("OsVersion")
                        .HasComment("The operating system version of the client making the request.");

                    b.Property<string>("RequestBody")
                        .HasColumnType("varchar")
                        .HasColumnName("RequestBody")
                        .HasComment("The body content of the request.");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("varchar")
                        .HasColumnName("RequestHeaders")
                        .HasComment("The headers included in the request.");

                    b.Property<string>("RequestMethod")
                        .HasMaxLength(10)
                        .HasColumnType("varchar")
                        .HasColumnName("RequestMethod")
                        .HasComment("The HTTP method used in the request (e.g., GET, POST).");

                    b.Property<string>("RequestPath")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("RequestPath")
                        .HasComment("The path of the requested resource.");

                    b.Property<string>("RequestTime")
                        .HasColumnType("datetime")
                        .HasColumnName("RequestTime")
                        .HasComment("The timestamp of when the request was made.");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("varchar")
                        .HasColumnName("ResponseBody")
                        .HasComment("The body content of the response.");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnType("varchar")
                        .HasColumnName("ResponseHeaders")
                        .HasComment("The headers included in the response.");

                    b.Property<string>("ResponseStatusCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar")
                        .HasColumnName("ResponseStatusCode")
                        .HasComment("The HTTP status code of the response.");

                    b.Property<string>("SourceName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("SourceName")
                        .HasComment("The source name of the request.");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Username")
                        .HasComment("The username of the user making the request.");

                    b.Property<Guid>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar")
                        .HasColumnName("RowVersion")
                        .HasComment("The version of the data entry.");

                    b.HasKey("Id")
                        .HasName("PK_UserActivity_UserActivityId");

                    b.HasIndex(new[] { "Id" }, "AK_UserActivity_UserActivityId")
                        .IsUnique();

                    b.ToTable("UserActivity", null, t =>
                        {
                            t.HasComment("A log entry for user activity within the system.");
                        });
                });

            modelBuilder.Entity("Services.Controllers.API.Database.Models.WeatherForecastDto", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar")
                        .HasColumnName("Id")
                        .HasComment("The unique identifier for the weather forecast.");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasComment("Who created the record.");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("date")
                        .HasColumnName("Date")
                        .HasComment("The date of the weather forecast.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("Summary")
                        .HasComment("A summary description of the weather (e.g., sunny, rainy).");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("integer")
                        .HasColumnName("TemperatureC")
                        .HasComment("The temperature in Celsius for the forecasted date.");

                    b.Property<int>("TemperatureF")
                        .HasColumnType("integer")
                        .HasColumnName("TemperatureF")
                        .HasComment("The temperature in Fahrenheit for the forecasted date, calculated from the temperature in Celsius.");

                    b.Property<Guid>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar")
                        .HasColumnName("RowVersion")
                        .HasComment("The version of the data entry.");

                    b.HasKey("Id")
                        .HasName("PK_WeatherForecast_WeatherForecastId");

                    b.HasIndex(new[] { "Id" }, "AK_WeatherForecast_WeatherForecastId")
                        .IsUnique();

                    b.ToTable("WeatherForecast", null, t =>
                        {
                            t.HasComment("A weather forecast for a specific date.");
                        });

                    b.HasData(
                        new
                        {
                            Id = "38b7942a-8a8f-4a34-9744-e4dea6eaed78",
                            CreatedBy = "System",
                            CreatedDate = new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = "2025-01-04",
                            Summary = "Hot",
                            TemperatureC = 25,
                            TemperatureF = 76,
                            Version = new Guid("38b7942a-8a8f-4a34-9744-e4dea6eaed78")
                        },
                        new
                        {
                            Id = "3db3a34a-9dcf-42e6-977f-d6bbb2329f16",
                            CreatedBy = "System",
                            CreatedDate = new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = "2025-01-07",
                            Summary = "Cool",
                            TemperatureC = 15,
                            TemperatureF = 58,
                            Version = new Guid("3db3a34a-9dcf-42e6-977f-d6bbb2329f16")
                        },
                        new
                        {
                            Id = "76d5e039-63b3-4c7f-bb8d-0847f729dcde",
                            CreatedBy = "System",
                            CreatedDate = new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = "2025-01-09",
                            Summary = "Cold",
                            TemperatureC = 5,
                            TemperatureF = 40,
                            Version = new Guid("76d5e039-63b3-4c7f-bb8d-0847f729dcde")
                        },
                        new
                        {
                            Id = "1130f076-1d75-4977-8a50-323a4ecf8f4e",
                            CreatedBy = "System",
                            CreatedDate = new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = "2025-01-11",
                            Summary = "Hot",
                            TemperatureC = 35,
                            TemperatureF = 94,
                            Version = new Guid("1130f076-1d75-4977-8a50-323a4ecf8f4e")
                        },
                        new
                        {
                            Id = "2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04",
                            CreatedBy = "System",
                            CreatedDate = new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = "2025-01-20",
                            Summary = "Warm",
                            TemperatureC = 20,
                            TemperatureF = 67,
                            Version = new Guid("2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04")
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
