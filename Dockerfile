# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG SERVICE
WORKDIR /src

COPY . .

# Restore and publish the specific service
WORKDIR /src/Services/${SERVICE}/${SERVICE}.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
ARG SERVICE
ENV SERVICE_NAME=${SERVICE}
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT dotnet ${SERVICE_NAME}.API.dll