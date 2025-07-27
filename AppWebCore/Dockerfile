# Usa una imagen base de .NET SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo .csproj para restaurar las dependencias (mejora el cacheo)
COPY ["AppWebCore/AppWebCore.csproj", "AppWebCore/"]
RUN dotnet restore "AppWebCore/AppWebCore.csproj"

# Copia todo el código fuente del proyecto
COPY . AppWebCore/
WORKDIR /src/AppWebCore

# Publica la aplicación
RUN dotnet publish "AppWebCore.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Usa una imagen de tiempo de ejecución más pequeña para la aplicación final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080 # Puerto por defecto para ASP.NET Core en Docker

# Copia la salida publicada de la etapa de construcción
COPY --from=build /app/publish .

# Define el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "AppWebCore.dll"]