FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/AppWebCore

COPY ["AppWebCore.csproj", "./"]
RUN dotnet restore "AppWebCore.csproj"

COPY . .

RUN dotnet publish "AppWebCore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AppWebCore.dll"]