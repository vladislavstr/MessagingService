# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Core/AspireService/AspireService.csproj", "Core/AspireService/"]
RUN dotnet restore "./Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG SSL_CERT_PASSWORD
# Creating a certificate at the publish stage
RUN mkdir -p /app/.aspnet/https/ && \
    dotnet dev-certs https -ep /app/.aspnet/https/aspnetapp.pfx -p $SSL_CERT_PASSWORD && \
    chmod 644 /app/.aspnet/https/aspnetapp.pfx
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copying the certificate from the publish stage
COPY --from=publish /app/.aspnet/https /app/.aspnet/https

# Set permissions and switch to non-root user
RUN chown -R $APP_UID:$APP_UID /app && \
    chmod -R 755 /app
USER $APP_UID

ENTRYPOINT ["dotnet", "Api.dll"]