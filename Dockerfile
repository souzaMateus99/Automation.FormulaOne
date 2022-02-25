FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY src/ ./
RUN dotnet restore Script.FormulaOneCalendar/Script.FormulaOneCalendar.csproj

RUN mkdir out
RUN dotnet publish -c Release -o out Script.FormulaOneCalendar/Script.FormulaOneCalendar.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS run-env
WORKDIR /app
COPY --from=build-env /app/out .

ENV ApplicationName "FormulaOneCalendar"
ENV FormulaOneApiUrl "http://ergast.com/api/f1/"
ENV FormulaOneApiVersion "2.0"
ENV FormulaOneApiLanguage "POR"
ENV FormulaOneApiYear "2022"
ENV FormulaOneApiPageId "4319"
ENV GoogleCalendarId ""
ENV ServiceAccountEmail ""
ENV ServiceAccountPrivateKey ""

ENTRYPOINT ["dotnet", "Script.FormulaOneCalendar.dll", "1"]