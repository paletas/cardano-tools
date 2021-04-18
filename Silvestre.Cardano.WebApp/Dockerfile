#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Silvestre.Cardano.WebApp/Silvestre.Cardano.WebApp.csproj", "Silvestre.Cardano.WebApp/"]
RUN dotnet restore "Silvestre.Cardano.WebApp/Silvestre.Cardano.WebApp.csproj"
COPY . .
WORKDIR "/src/Silvestre.Cardano.WebApp"
RUN dotnet build "Silvestre.Cardano.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Silvestre.Cardano.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Silvestre.Cardano.WebApp.dll"]