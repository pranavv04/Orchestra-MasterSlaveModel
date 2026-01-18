FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y \
    bash \
    g++ \
    build-essential

COPY bin/Release/net8.0/ .

ENTRYPOINT ["dotnet", "Orchestra.Worker.dll"]
