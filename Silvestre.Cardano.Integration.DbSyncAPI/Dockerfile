#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Silvestre.Cardano.Integration.DbSyncAPI/Silvestre.Cardano.Integration.DbSyncAPI.csproj", "Silvestre.Cardano.Integration.DbSyncAPI/"]
RUN dotnet restore "Silvestre.Cardano.Integration.DbSyncAPI/Silvestre.Cardano.Integration.DbSyncAPI.csproj"
COPY . .
WORKDIR "/src/Silvestre.Cardano.Integration.DbSyncAPI"
RUN dotnet build "Silvestre.Cardano.Integration.DbSyncAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Silvestre.Cardano.Integration.DbSyncAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Silvestre.Cardano.Integration.DbSyncAPI.dll"]