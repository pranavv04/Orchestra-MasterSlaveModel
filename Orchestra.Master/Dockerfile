FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY bin/Release/net8.0/ .
EXPOSE 5216

ENTRYPOINT ["dotnet", "Orchestra.Master.dll"]
