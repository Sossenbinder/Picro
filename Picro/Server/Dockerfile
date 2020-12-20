#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Picro/Server/Picro.Server.csproj", "Picro/Server/"]
COPY ["Picro/Shared/Picro.Shared.csproj", "Picro/Shared/"]
COPY ["Picro/Client/Picro.Client.csproj", "Picro/Client/"]
RUN dotnet restore "Picro/Server/Picro.Server.csproj"
COPY . .
WORKDIR "/src/Picro/Server"
RUN dotnet build "Picro.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Picro.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Picro.Server.dll"]