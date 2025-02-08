# Microservices Minimal API

<!-- ## Docker
Notes: From the Publications folder run...

1) Build the image
```bash
docker build -f Dockerfile.Publications.Authors --tag publications.authors .
```

2) Create and run the container
```bash
docker run --name publications.authors -p 7000:80 -p 7001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=7001 publications.authors
``` 

## Nuget

### Clear Cash
```bash
nuget locals all -clear
```
### Create nuget package
Notes: from project directory
```bash
dotnet pack -o ..\packages\
```

## Simple Certificate
```bash
dotnet dev-certs https --trust
```

## Certificate
```bash
$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dns publications.io
$pwd = ConvertTo-SecureString -String "Ci281978!" -Force -AsPlainText
$certpath = "Cert:\localmachine\my\$($cert.Thumbprint)"
Export-PfxCertificate -Cert $certpath -FilePath C:\Users\ciordanidis\.aspnet\https\publications.io.pfx -Password $pwd
dotnet user-secrets set "CertPassword" "Ci281978!"
dotnet dev-certs https --trust
```

## Seq logging database
  1. Username admin
  2. Password root

## Swagger
  Path: {Domain}/swagger/index.html

-->
# Entity Framework

## Entity Framework Migrations
```bash
dotnet-ef migrations add -o Database/Migrations InitialCreate -v
```
## Entity Framework Update Database
```bash
dotnet-ef database update -v
```
## Entity Framework Drop Database
```bash
dotnet-ef database drop -v
```
## Entity Framework Core Database Provider Preprocessor Directives

| Database Provider            | Preprocessor Directive |
|------------------------------|------------------------|
| Microsoft SQL Server         | `#if NETSTANDARD` (general) |
| SQLite                       | `#if NETSTANDARD` (general) |
| PostgreSQL (Npgsql)          | `#if NPGSQL` |
| MySQL (Pomelo)               | `#if MYSQL` |
| Oracle (Oracle.EntityFrameworkCore) | `#if ORACLE` |
| Cosmos DB                    | `#if COSMOS` |
| In-Memory Database           | `#if MEMORY` |

### Usage Example
When writing provider-specific logic, you can use preprocessor directives like this:
```csharp
#if NPGSQL
    optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;");
#elif MYSQL
    optionsBuilder.UseMySql("server=my_server;",
        new MySqlServerVersion(new Version(8, 0, 23)));
#elif ORACLE
    optionsBuilder.UseOracle("User Id=my_user;");
#else
    optionsBuilder.UseSqlServer("Server=my_server;");
#endif
```
## Benchmarking
Open CLI and type
```cmd
dotnet run -c release 
```
```cmd
curl -X 'GET' 'http://localhost:5096/api/v1/WeatherForecast/benchmark' -H 'accept: application/json' 
```
## K6 Test
  Open CLI and type...
```bash
k6 run --duration 10s .\K6\test-script.js
```