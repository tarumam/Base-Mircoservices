FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

COPY ["./buildingBlocks/BaseProject.WebAPI.Core/BaseProject.WebAPI.Core.csproj", "buildingBlocks/BaseProject.WebAPI.Core/"]
COPY ["./buildingBlocks/BaseProject.Core/BaseProject.Core.csproj", "buildingBlocks/BaseProject.Core/"]
COPY ["./buildingBlocks/BaseProject.MessageBus/BaseProject.MessageBus.csproj", "buildingBlocks/BaseProject.MessageBus/"]
COPY ["./services/BaseProject.Seller.API/BaseProject.Seller.API.csproj", "services/BaseProject.Seller.API/"]

RUN dotnet restore "services/BaseProject.Seller.API/BaseProject.Seller.API.csproj"
COPY . .
WORKDIR "services/BaseProject.Seller.API"
RUN dotnet build "BaseProject.Seller.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseProject.Seller.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet BaseProject.Seller.API.dll
