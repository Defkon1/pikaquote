#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update && apt-get install -y fortune-mod
RUN apt-get install -y wget
RUN apt-get install -y dos2unix
RUN wget --no-check-certificate https://pastebin.com/raw/p296KDcE -O /usr/games/fortune-devops
RUN wget --no-check-certificate https://pastebin.com/raw/rHtSXFbx -O /usr/games/fortune-murphy-it
RUN dos2unix /usr/games/fortune-devops
RUN dos2unix /usr/games/fortune-murphy-it
RUN strfile /usr/games/fortune-devops
RUN strfile /usr/games/fortune-murphy-it

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Pikaquote/Pikaquote.csproj", "Pikaquote/"]
RUN dotnet restore "Pikaquote/Pikaquote.csproj"
COPY . .
WORKDIR "/src/Pikaquote"
RUN dotnet build "Pikaquote.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pikaquote.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pikaquote.dll"]