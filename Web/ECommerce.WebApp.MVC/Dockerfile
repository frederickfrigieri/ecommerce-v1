#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ECommerce.WebApp.MVC/ECommerce.WebApp.MVC.csproj", "ECommerce.WebApp.MVC/"]
RUN dotnet restore "ECommerce.WebApp.MVC/ECommerce.WebApp.MVC.csproj"
COPY . .
WORKDIR "/src/ECommerce.WebApp.MVC"
RUN dotnet build "ECommerce.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.WebApp.MVC.dll"]