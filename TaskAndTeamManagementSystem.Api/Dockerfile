# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskAndTeamManagementSystem.Api/TaskAndTeamManagementSystem.Api.csproj", "TaskAndTeamManagementSystem.Api/"]
COPY ["TaskAndTeamManagementSystem.Identity/TaskAndTeamManagementSystem.Identity.csproj", "TaskAndTeamManagementSystem.Identity/"]
COPY ["TaskAndTeamManagementSystem.Application/TaskAndTeamManagementSystem.Application.csproj", "TaskAndTeamManagementSystem.Application/"]
COPY ["TaskAndTeamManagementSystem.Contracts/TaskAndTeamManagementSystem.Contracts.csproj", "TaskAndTeamManagementSystem.Contracts/"]
COPY ["TaskAndTeamManagementSystem.Domain/TaskAndTeamManagementSystem.Domain.csproj", "TaskAndTeamManagementSystem.Domain/"]
COPY ["TaskAndTeamManagementSystem.Shared/TaskAndTeamManagementSystem.Shared.csproj", "TaskAndTeamManagementSystem.Shared/"]
COPY ["TaskAndTeamManagementSystem.Persistence/TaskAndTeamManagementSystem.Persistence.csproj", "TaskAndTeamManagementSystem.Persistence/"]
COPY ["TaskAndTeamManagementSystem.Infrastructure/TaskAndTeamManagementSystem.Infrastructure.csproj", "TaskAndTeamManagementSystem.Infrastructure/"]
RUN dotnet restore "./TaskAndTeamManagementSystem.Api/TaskAndTeamManagementSystem.Api.csproj"
COPY . .
WORKDIR "/src/TaskAndTeamManagementSystem.Api"
RUN dotnet build "./TaskAndTeamManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskAndTeamManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "TaskAndTeamManagementSystem.Api.dll"]