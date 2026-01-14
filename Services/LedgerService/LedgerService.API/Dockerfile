# LedgerService Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Project Files
COPY ["Services/LedgerService/LedgerService.API/LedgerService.API.csproj", "Services/LedgerService/LedgerService.API/"]
COPY ["Services/LedgerService/LedgerService.Application/LedgerService.Application.csproj", "Services/LedgerService/LedgerService.Application/"]
COPY ["Services/LedgerService/LedgerService.Domain/LedgerService.Domain.csproj", "Services/LedgerService/LedgerService.Domain/"]
COPY ["Services/LedgerService/LedgerService.Infrastructure/LedgerService.Infrastructure.csproj", "Services/LedgerService/LedgerService.Infrastructure/"]
COPY ["Common/Messaging/Messaging.csproj", "Common/Messaging/"]
COPY ["Common/Messaging.Persistence/Messaging.Persistence.csproj", "Common/Messaging.Persistence/"]
COPY ["Common/ServiceDefaults/ServiceDefaults.csproj", "Common/ServiceDefaults/"]

# Restore Dependencies
RUN dotnet restore "Services/LedgerService/LedgerService.API/LedgerService.API.csproj"

# Copy Remaining Files
COPY . .

# Build and Publish
WORKDIR "/src/Services/LedgerService/LedgerService.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LedgerService.API.dll"]
