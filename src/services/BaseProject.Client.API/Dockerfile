FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY ["./buildingBlocks/BaseProject.WebAPI.Core/BaseProject.WebAPI.Core.csproj", "buildingBlocks/BaseProject.WebAPI.Core/"]
COPY ["./buildingBlocks/BaseProject.Core/BaseProject.Core.csproj", "buildingBlocks/BaseProject.Core/"]
COPY ["./buildingBlocks/BaseProject.MessageBus/BaseProject.MessageBus.csproj", "buildingBlocks/BaseProject.MessageBus/"]
COPY ["./services/BaseProject.Client.API/BaseProject.Clients.API.csproj", "services/BaseProject.Client.API/"]

RUN dotnet restore "services/BaseProject.Client.API/BaseProject.Clients.API.csproj"
COPY . .
WORKDIR "services/BaseProject.Client.API"
RUN dotnet build "BaseProject.Clients.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseProject.Clients.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet BaseProject.Clients.API.dll