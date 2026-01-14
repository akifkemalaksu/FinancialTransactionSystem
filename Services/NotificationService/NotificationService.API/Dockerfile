# NotificationService Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Project Files
COPY ["Services/NotificationService/NotificationService.API/NotificationService.API.csproj", "Services/NotificationService/NotificationService.API/"]
COPY ["Services/NotificationService/NotificationService.Application/NotificationService.Application.csproj", "Services/NotificationService/NotificationService.Application/"]
COPY ["Services/NotificationService/NotificationService.Domain/NotificationService.Domain.csproj", "Services/NotificationService/NotificationService.Domain/"]
COPY ["Services/NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "Services/NotificationService/NotificationService.Infrastructure/"]
COPY ["Common/Messaging/Messaging.csproj", "Common/Messaging/"]
COPY ["Common/Messaging.Persistence/Messaging.Persistence.csproj", "Common/Messaging.Persistence/"]
COPY ["Common/ServiceDefaults/ServiceDefaults.csproj", "Common/ServiceDefaults/"]

# Restore Dependencies
RUN dotnet restore "Services/NotificationService/NotificationService.API/NotificationService.API.csproj"

# Copy Remaining Files
COPY . .

# Build and Publish
WORKDIR "/src/Services/NotificationService/NotificationService.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
