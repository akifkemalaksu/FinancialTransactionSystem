# TransactionService Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Project Files
COPY ["Services/TransactionService/TransactionService.API/TransactionService.API.csproj", "Services/TransactionService/TransactionService.API/"]
COPY ["Services/TransactionService/TransactionService.Application/TransactionService.Application.csproj", "Services/TransactionService/TransactionService.Application/"]
COPY ["Services/TransactionService/TransactionService.Domain/TransactionService.Domain.csproj", "Services/TransactionService/TransactionService.Domain/"]
COPY ["Services/TransactionService/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj", "Services/TransactionService/TransactionService.Infrastructure/"]
COPY ["Common/Messaging/Messaging.csproj", "Common/Messaging/"]
COPY ["Common/Messaging.Persistence/Messaging.Persistence.csproj", "Common/Messaging.Persistence/"]
COPY ["Common/ServiceDefaults/ServiceDefaults.csproj", "Common/ServiceDefaults/"]

# Restore Dependencies
RUN dotnet restore "Services/TransactionService/TransactionService.API/TransactionService.API.csproj"

# Copy Remaining Files
COPY . .

# Build and Publish
WORKDIR "/src/Services/TransactionService/TransactionService.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TransactionService.API.dll"]
