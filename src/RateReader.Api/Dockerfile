FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY RateReader.Api/RateReader.Api.csproj RateReader.Api/
RUN dotnet restore RateReader.Api/RateReader.Api.csproj
COPY . .
WORKDIR /src/RateReader.Api
RUN dotnet build RateReader.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RateReader.Api.dll"]
