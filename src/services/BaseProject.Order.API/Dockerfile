
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BaseProject.Order.API.csproj", "./"]
RUN dotnet restore "./BaseProject.Order.API.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "BaseProject.Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseProject.Order.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BaseProject.Order.API.dll
