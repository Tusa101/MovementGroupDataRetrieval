#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/DataRetrieval/DataRetrieval.WebAPI/DataRetrieval.WebAPI.csproj", "src/Services/DataRetrieval/DataRetrieval.WebAPI/"]
RUN dotnet restore "./src/Services/DataRetrieval/DataRetrieval.WebAPI/DataRetrieval.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/DataRetrieval/DataRetrieval.WebAPI"
RUN dotnet build "./DataRetrieval.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DataRetrieval.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataRetrieval.WebAPI.dll"]