# AccountService Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Project Files
COPY ["Services/AccountService/AccountService.API/AccountService.API.csproj", "Services/AccountService/AccountService.API/"]
COPY ["Services/AccountService/AccountService.Application/AccountService.Application.csproj", "Services/AccountService/AccountService.Application/"]
COPY ["Services/AccountService/AccountService.Domain/AccountService.Domain.csproj", "Services/AccountService/AccountService.Domain/"]
COPY ["Services/AccountService/AccountService.Infrastructure/AccountService.Infrastructure.csproj", "Services/AccountService/AccountService.Infrastructure/"]
COPY ["Common/Messaging/Messaging.csproj", "Common/Messaging/"]
COPY ["Common/Messaging.Persistence/Messaging.Persistence.csproj", "Common/Messaging.Persistence/"]
COPY ["Common/ServiceDefaults/ServiceDefaults.csproj", "Common/ServiceDefaults/"]

# Restore Dependencies
RUN dotnet restore "Services/AccountService/AccountService.API/AccountService.API.csproj"

# Copy Remaining Files
COPY . .

# Build and Publish
WORKDIR "/src/Services/AccountService/AccountService.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AccountService.API.dll"]
