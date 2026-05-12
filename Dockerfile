FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ECommerceApi.sln .
COPY src/ECommerceApi.Domain/ECommerceApi.Domain.csproj src/ECommerceApi.Domain/
COPY src/ECommerceApi.Application/ECommerceApi.Application.csproj src/ECommerceApi.Application/
COPY src/ECommerceApi.Infrastructure/ECommerceApi.Infrastructure.csproj src/ECommerceApi.Infrastructure/
COPY src/ECommerceApi.API/ECommerceApi.API.csproj src/ECommerceApi.API/

RUN dotnet restore ECommerceApi.sln

COPY src/ src/

RUN dotnet publish src/ECommerceApi.API/ECommerceApi.API.csproj -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerceApi.API.dll"]
