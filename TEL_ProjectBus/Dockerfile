# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY . ./
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "TEL_ProjectBus.dll"]
