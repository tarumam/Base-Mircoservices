FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BaseProject.Payment.API.csproj", "src/services/BaseProject.Payment.API/"]
RUN dotnet restore "src/services/BaseProject.Payment.API/BaseProject.Payment.API.csproj"
COPY . .
WORKDIR "/src/src/services/BaseProject.Payment.API"
RUN dotnet build "BaseProject.Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseProject.Payment.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaseProject.Payment.API.dll"]