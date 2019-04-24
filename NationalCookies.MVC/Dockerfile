FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["NationalCookies.MVC/NationalCookies.MVC.csproj", "NationalCookies.MVC/"]
COPY ["NationalCookies.Data/NationalCookies.Data.csproj", "NationalCookies.Data/"]
RUN dotnet restore "NationalCookies.MVC/NationalCookies.MVC.csproj"
COPY . .
WORKDIR "/src/NationalCookies.MVC"
RUN dotnet build "NationalCookies.MVC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NationalCookies.MVC.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NationalCookies.MVC.dll"]
