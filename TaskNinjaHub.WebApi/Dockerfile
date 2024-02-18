#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskNinjaHub.WebApi/TaskNinjaHub.WebApi.csproj", "TaskNinjaHub.WebApi/"]
COPY ["TaskNinjaHub.Application/TaskNinjaHub.Application.csproj", "TaskNinjaHub.Application/"]
COPY ["TaskNinjaHub.Persistence/TaskNinjaHub.Persistence.csproj", "TaskNinjaHub.Persistence/"]
RUN dotnet restore "./TaskNinjaHub.WebApi/./TaskNinjaHub.WebApi.csproj"
COPY . .
WORKDIR "/src/TaskNinjaHub.WebApi"
RUN dotnet build "./TaskNinjaHub.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskNinjaHub.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskNinjaHub.WebApi.dll"]