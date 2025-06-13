# 1. Stage — Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY EventWebApp/*.csproj ./EventWebApp/
COPY EventWebApp.Application/*.csproj ./EventWebApp.Application/
COPY EventWebApp.Core/*.csproj ./EventWebApp.Core/
COPY EventWebApp.Infrastructure/*.csproj ./EventWebApp.Infrastructure/
COPY EventWebApp.WebAPI/*.csproj ./EventWebApp.WebAPI/

RUN dotnet restore ./EventWebApp.WebAPI/EventWebApp.WebAPI.csproj

COPY . .
WORKDIR /app/EventWebApp.WebAPI
RUN dotnet publish -c Release -o /out

# 2. Stage — Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "EventWebApp.WebAPI.dll"]
