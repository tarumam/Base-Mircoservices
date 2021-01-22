FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY ["./buildingBlocks/BaseProject.WebAPI.Core/BaseProject.WebAPI.Core.csproj", "buildingBlocks/BaseProject.WebAPI.Core/"]
COPY ["./buildingBlocks/BaseProject.Core/BaseProject.Core.csproj", "buildingBlocks/BaseProject.Core/"]
COPY ["./buildingBlocks/BaseProject.MessageBus/BaseProject.MessageBus.csproj", "buildingBlocks/BaseProject.MessageBus/"]
COPY ["./services/BaseProject.Identity.API/BaseProject.Identity.API.csproj", "services/BaseProject.Identity.API/"]

RUN dotnet restore "services/BaseProject.Identity.API/BaseProject.Identity.API.csproj"
COPY . .
WORKDIR "services/BaseProject.Identity.API"
RUN dotnet build "BaseProject.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseProject.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet BaseProject.Identity.API.dll