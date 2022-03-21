FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ConsoleGame/ConsoleGame.csproj", "ConsoleGame/"]
RUN dotnet restore "ConsoleGame/ConsoleGame.csproj"
COPY . .
WORKDIR "/src/ConsoleGame"
RUN dotnet build "ConsoleGame.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleGame.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleGame.dll"]
