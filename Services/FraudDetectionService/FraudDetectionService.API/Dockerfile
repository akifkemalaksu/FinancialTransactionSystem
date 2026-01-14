# FraudDetectionService Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Project Files
COPY ["Services/FraudDetectionService/FraudDetectionService.API/FraudDetectionService.API.csproj", "Services/FraudDetectionService/FraudDetectionService.API/"]
COPY ["Services/FraudDetectionService/FraudDetectionService.Application/FraudDetectionService.Application.csproj", "Services/FraudDetectionService/FraudDetectionService.Application/"]
COPY ["Common/ServiceDefaults/ServiceDefaults.csproj", "Common/ServiceDefaults/"]

# Restore Dependencies
RUN dotnet restore "Services/FraudDetectionService/FraudDetectionService.API/FraudDetectionService.API.csproj"

# Copy Remaining Files
COPY . .

# Build and Publish
WORKDIR "/src/Services/FraudDetectionService/FraudDetectionService.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FraudDetectionService.API.dll"]
