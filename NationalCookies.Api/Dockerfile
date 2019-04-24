FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["NationalCookies.Api/NationalCookies.Api.csproj", "NationalCookies.Api/"]
RUN dotnet restore "NationalCookies.Api/NationalCookies.Api.csproj"
COPY . .
WORKDIR "/src/NationalCookies.Api"
RUN dotnet build "NationalCookies.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NationalCookies.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NationalCookies.Api.dll"]
