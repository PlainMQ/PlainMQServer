FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as base

WORKDIR /app

COPY ./bin/Release/net6.0/publish .

EXPOSE 13000

ENTRYPOINT [ "dotnet", "PlainMQServer.dll" ]