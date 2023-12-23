#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["MicUI.Configuration/MicUI.Configuration.csproj", "MicUI.Configuration/"]
COPY ["BPA.GlobalResources/BPA.GlobalResources.csproj", "BPA.GlobalResources/"]
RUN dotnet restore "./MicUI.Configuration/./MicUI.Configuration.csproj"
COPY . .
WORKDIR "/src/MicUI.Configuration"
RUN dotnet build "./MicUI.Configuration.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicUI.Configuration.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicUI.Configuration.dll"]