FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app


COPY *.sln ./
COPY src/ ./src
RUN dotnet restore /app/src/CryptoQuote.API


WORKDIR /app/src/CryptoQuote.API
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "CryptoQuote.API.dll"]
