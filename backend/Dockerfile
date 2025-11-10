# Million Luxury Backend - Production Dockerfile
# Multi-stage build for optimized image size

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["MillionBackend.sln", "./"]
COPY ["src/MillionBackend.API/MillionBackend.API.csproj", "src/MillionBackend.API/"]
COPY ["src/MillionBackend.Application/MillionBackend.Application.csproj", "src/MillionBackend.Application/"]
COPY ["src/MillionBackend.Core/MillionBackend.Core.csproj", "src/MillionBackend.Core/"]
COPY ["src/MillionBackend.Infrastructure/MillionBackend.Infrastructure.csproj", "src/MillionBackend.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "MillionBackend.sln"

# Copy all source code
COPY . .

# Build
WORKDIR "/src/src/MillionBackend.API"
RUN dotnet build "MillionBackend.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "MillionBackend.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=publish /app/publish .

# Create non-root user
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "MillionBackend.API.dll"]
