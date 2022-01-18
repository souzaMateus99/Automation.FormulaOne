FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY src/Script.FormulaOneCalendar/*.csproj ./
RUN dotnet restore

COPY src/ ./
RUN mkdir out
RUN dotnet publish -c Release -o out Script.FormulaOneCalendar/Script.FormulaOneCalendar.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS run-env
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Script.FormulaOneCalendar.dll"]