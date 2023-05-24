FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 12000
ENV ASPNETCORE_ENVIRONMENT=Staging

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gateway.Application/Gateway.Application.csproj", "./Gateway.Application/"]
COPY ["Gateway.Application.Shared/Gateway.Application.Shared.csproj", "./Gateway.Application.Shared/"]
COPY ["Gateway.Core/Gateway.Core.csproj", "./Gateway.Core/"]
COPY ["Gateway.Core.Shared/Gateway.Core.Shared.csproj", "./Gateway.Core.Shared/"]
COPY ["Gateway.Domain/Gateway.Domain.csproj", "./Gateway.Domain/"]
COPY ["Gateway.EntityFrameworkCore/Gateway.EntityFrameworkCore.csproj", "./Gateway.EntityFrameworkCore/"]
COPY ["Gateway.Web.Host/Gateway.Web.Host.csproj", "./Gateway.Web.Host/"]
RUN dotnet restore "./Gateway.Web.Host/Gateway.Web.Host.csproj"
COPY . .

WORKDIR "/src/Gateway.Web.Host"
RUN dotnet build "./Gateway.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Web.Host.dll"]
