# Base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
USER $APP_UID

# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/Services/ProductService/ProductService.csproj", "src/Services/ProductService/"]
COPY ["src/BuildingBlocks/BuildingBlocks.csproj", "src/BuildingBlocks/"]
RUN dotnet restore "./src/Services/ProductService/ProductService.csproj"

# Copy source
COPY . .
WORKDIR "/src/src/Services/ProductService"

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductService.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/Shared/Scripts/wait-for", "./"]
COPY ["src/Services/ProductService/docker-entrypoint.sh", "./"]
USER root
RUN chmod +x ./docker-entrypoint.sh ./wait-for
USER $APP_UID
ENTRYPOINT ["./docker-entrypoint.sh"]