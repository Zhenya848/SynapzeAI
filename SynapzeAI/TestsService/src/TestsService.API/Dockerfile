FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG NUGET_USERNAME
ARG NUGET_PASSWORD
WORKDIR /src

RUN dotnet nuget add source https://nuget.pkg.github.com/Zhenya848/index.json --name github \
    --username $NUGET_USERNAME --password $NUGET_PASSWORD --store-password-in-clear-text

COPY ["src/TestsService.Presentation/TestsService.Presentation.csproj", "src/TestsService.Presentation/"]
COPY ["src/TestsService.API/TestsService.API.csproj", "src/TestsService.API/"]
COPY ["src/TestsService.Application/TestsService.Application.csproj", "src/TestsService.Application/"]
COPY ["src/TestsService.Domain/TestsService.Domain.csproj", "src/TestsService.Domain/"]
COPY ["src/TestsService.Infrastructure/TestsService.Infrastructure.csproj", "src/TestsService.Infrastructure/"]

RUN dotnet restore "src/TestsService.API/TestsService.API.csproj"

COPY . .
WORKDIR "/src/src/TestsService.API"
RUN dotnet build "TestsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TestsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestsService.API.dll"]