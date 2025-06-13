FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["EventWebApp.WebAPI/EventWebApp.WebAPI.csproj", "EventWebApp.WebAPI/"]
COPY ["EventWebApp.Application/EventWebApp.Application.csproj", "EventWebApp.Application/"]
COPY ["EventWebApp.Core/EventWebApp.Core.csproj", "EventWebApp.Core/"]
COPY ["EventWebApp.Infrastructure/EventWebApp.Infrastructure.csproj", "EventWebApp.Infrastructure/"]
RUN dotnet restore "EventWebApp.WebAPI/EventWebApp.WebAPI.csproj"
COPY . .
WORKDIR "/src/EventWebApp.WebAPI"
RUN dotnet build "EventWebApp.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventWebApp.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventWebApp.WebAPI.dll"]
