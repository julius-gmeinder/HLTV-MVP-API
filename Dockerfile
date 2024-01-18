#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y xorg openbox libnss3 libasound2

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HLTV-API/HLTV-API.csproj", "HLTV-API/"]
RUN dotnet restore "HLTV-API/HLTV-API.csproj"
COPY . .
WORKDIR "/src/HLTV-API"
RUN dotnet build "HLTV-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HLTV-API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HLTV-API.dll"]