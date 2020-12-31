#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Picro/Server/Picro.Server.csproj", "Picro/Server/"]
COPY ["Picro/Client/Picro.Client.csproj", "Picro/Client/"]
COPY ["Picro/Common/Picro.Common/Picro.Common.csproj", "Picro/Common/Picro.Common/"]
COPY ["Picro/Common/Picro.Common.Web/Picro.Common.Web.csproj", "Picro/Common/Picro.Common.Web/"]
COPY ["Picro/Common/Modules/Picro.Module.Identity/Picro.Module.Identity.csproj", "Picro/Common/Modules/Picro.Module.Identity/"]
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