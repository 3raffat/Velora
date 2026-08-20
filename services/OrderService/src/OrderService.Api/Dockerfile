FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Velora.Api/Velora.Api.csproj", "src/Velora.Api/"]
COPY ["src/Velora.Application/Velora.Application.csproj", "src/Velora.Application/"]
COPY ["src/OrderService.Domain/OrderService.Domain.csproj", "src/OrderService.Domain/"]
COPY ["src/Velora.Infrastructure/Velora.Infrastructure.csproj", "src/Velora.Infrastructure/"]

RUN dotnet restore "./src/Velora.Api/Velora.Api.csproj"

COPY . .

WORKDIR "/src/src/Velora.Api"

RUN dotnet build "./Velora.Api.csproj" -c Release -o /app/build


FROM build AS publish

RUN dotnet publish "./Velora.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Velora.Api.dll"]