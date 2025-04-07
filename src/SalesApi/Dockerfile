# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia os projetos
COPY SalesApi.Application/*.csproj ./SalesApi.Application/
COPY SalesApi.Domain/*.csproj ./SalesApi.Domain/
COPY SalesApi.Infrastructure/*.csproj ./SalesApi.Infrastructure/
COPY SalesApi/*.csproj ./SalesApi/

# Vai para a pasta certa
WORKDIR /app/SalesApi

# Restaura dependências
RUN dotnet restore

# Volta e copia tudo
WORKDIR /app
COPY SalesApi.Application ./SalesApi.Application
COPY SalesApi.Domain ./SalesApi.Domain
COPY SalesApi.Infrastructure ./SalesApi.Infrastructure
COPY SalesApi ./SalesApi

# Volta para a API para publicar
WORKDIR /app/SalesApi
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/SalesApi/out .

ENTRYPOINT ["dotnet", "SalesApi.dll"]
