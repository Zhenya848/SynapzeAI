FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN adduser --disabled-password --home /app --uid 10000 appuser
RUN mkdir -p /app/etc && chown -R appuser:appuser /app/etc

USER appuser
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG NUGET_USERNAME
ARG NUGET_PASSWORD
WORKDIR /src

RUN dotnet nuget add source https://nuget.pkg.github.com/Zhenya848/index.json --name github \
    --username $NUGET_USERNAME --password $NUGET_PASSWORD --store-password-in-clear-text

COPY ["src/UserService.API/UserService.API.csproj", "src/UserService.API/"]
COPY ["src/UserService.Application/UserService.Application.csproj", "src/UserService.Application/"]
COPY ["src/UserService.Domain/UserService.Domain.csproj", "src/UserService.Domain/"]
COPY ["src/UserService.Infrastructure/UserService.Infrastructure.csproj", "src/UserService.Infrastructure/"]
COPY ["src/UserService.Presentation/UserService.Presentation.csproj", "src/UserService.Presentation/"]

RUN dotnet restore "src/UserService.API/UserService.API.csproj"

COPY . .
WORKDIR "/src/src/UserService.API"
RUN dotnet build "./UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish --chown=appuser:appuser /app/publish .
RUN mkdir -p /app/etc && chmod 755 /app/etc

ENTRYPOINT ["dotnet", "UserService.API.dll"]