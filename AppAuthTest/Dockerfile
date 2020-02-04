FROM mcr.microsoft.com/dotnet/core/runtime:3.1
ARG source=./out

WORKDIR /app

COPY ${source} .
ENTRYPOINT ["dotnet", "appauthtest.dll"]
