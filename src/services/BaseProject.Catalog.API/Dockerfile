FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

COPY ["./services/BaseProject.Catalog.API/BaseProject.Catalog.API.csproj", "services/BaseProject.Catalog.API/"]
COPY ["./buildingBlocks/BaseProject.Core/BaseProject.Core.csproj", "buildingBlocks/BaseProject.Core/"]
COPY ["./buildingBlocks/BaseProject.WebAPI.Core/BaseProject.WebAPI.Core.csproj", "buildingBlocks/BaseProject.WebAPI.Core/"]

RUN dotnet restore "services/BaseProject.Catalog.API/BaseProject.Catalog.API.csproj"
COPY . .
WORKDIR "services/BaseProject.Catalog.API"
RUN dotnet build "BaseProject.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseProject.Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#Padrão do container ASP.NET
#ENTRYPOINT ["dotnet", "BaseProject.Catalog.API.dll"]

#Opção de entrypoint recomendado pelo HEROKU
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BaseProject.Catalog.API.dll