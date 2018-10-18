FROM microsoft/dotnet:2.1-sdk
WORKDIR /app

ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY RateReader.Scheduler/RateReader.Scheduler.csproj RateReader.Scheduler/
RUN dotnet restore RateReader.Scheduler/RateReader.Scheduler.csproj
COPY . .
WORKDIR /src/RateReader.Scheduler
RUN dotnet build RateReader.Scheduler.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RateReader.Scheduler.dll"]