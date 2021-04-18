#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ECommerce.Cliente.API/ECommerce.Cliente.API.csproj", "ECommerce.Cliente.API/"]
RUN dotnet restore "ECommerce.Cliente.API/ECommerce.Cliente.API.csproj"
COPY . .
WORKDIR "/src/ECommerce.Cliente.API"
RUN dotnet build "ECommerce.Cliente.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.Cliente.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.Cliente.API.dll"]