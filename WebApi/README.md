# Portfolio WebApi .NET 7

> [JWT.io](https://jwt.io/)

> [Generate SHA256 Key](https://tools.keycdn.com/sha256-online-generator)

## Dependencies
```
ClassLibrary_Services
Microsoft.EntityFrameworkCore.SqlServer
```

## Structure
* **WebApi:** handles HTTP requests.
* **Application:** business logic and services.
* **Domain:** entities and business rules.
* **Infrastructure:** Interacts with databases and external services.
```
MySolution.sln
  ├── WebApi/                       (Web API)
  │   ├── Controllers/              (Controladores de la API)
  │   ├── Program.cs                (Configuración de la aplicación)
  │   └── appsettings.json          (Configuraciones)
  ├── Application/                  (Aplicación Class Library)
  │   ├── DTOs/                     (Objetos de Transferencia de Datos)
  │   ├── Entities/                 (Entidades de dominio)
  │   ├── Filters/                  (Filtros para la lógica de aplicación)
  │   ├── Interfaces/               (Interfaces de servicios)
  │   └── Services/                 (Implementaciones de servicios)
  ├── Domain/                       (Proyecto Class Library)
  │   ├── Entities/                 (Entidades del dominio)
  │   └── ValueObjects/             (Objetos de valor del dominio)
  ├── Infrastructure/               (Proyecto Class Library)
      ├── Data/                     (Manejo de datos)
      ├── Repositories/             (Repositorios)
      └── ExternalServices/         (Servicios externos)
```

## Loggers
* Use in Services and Controllers
```
private readonly ILogger<MyClass> _logger;

_logger.LogInformation("Iniciando la recuperación de grupos de URLs.");
_logger.LogWarning("Error al recuperar grupos de URLs: {StatusCode}", response.StatusCode);
_logger.LogError(ex, "Ocurrió un error al procesar la solicitud.");

_logger.LogCritical("Error crítico: La aplicación no puede continuar.");
_logger.LogDebug("Este es un mensaje de depuración.");
```

## Cloud and DevObs
### Docker
* Create Dockerfile
```
# Usa la imagen base de .NET
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["ClassLibraryApplication/ClassLibraryApplication.csproj", "ClassLibraryApplication/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

# Genera la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
```
* Create .dockerignore
```
bin/
obj/
.vscode/
.git/
```
* Build the Docker image
```
docker build -t nombre-de-tu-imagen .
```

### docker-compose
```
version: '3.8'

services:
  mssql:
    image: mcr.microsoft.com/mssql/server
    container_name: mssql_container    
    environment:
      - MSSQL_SA_PASSWORD=S!cretPa55
      - ACCEPT_EULA=Y
      - MSSQL_PID=Developer
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql

  webapi:
    build: .
    container_name: dotnet_webapi_container
    ports:
      - "8080:80"
    environment:
      ConnectionStrings__RutaWebSQL: "Server=mssql;Database=db_testing;User ID=testing;Password=testing;TrustServerCertificate=True;"
    depends_on:
      - mssql

volumes:
  mssql_data:
```
* [http://localhost:8080](http://localhost:8080)

* Build Compose
```
docker-compose up --build
```
