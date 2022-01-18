# Formula One Calendar

> ℹ️ Caso queira ler o README em português brasileiro, [clique aqui](docs/README.pt.md)<p>
> (If you want to read README in brazilian portuguese, [click here](docs/README.pt.md))

> ⚠️ [Click here](https://calendar.google.com/calendar/u/0?cid=ZjdycmM5b2V1bmVhbWhpN2dnMzlic29kczBAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ) if you want the F1 season without run this app. 
> <p>The link will add you in a public calendar with the F1 races</p>

This project is an automation that add, update and remove the F1 races to a Google Calendar.<p>
The project uses the [Ergast Api](http://ergast.com/mrd/) ([Postman documentation](https://documenter.getpostman.com/view/11586746/SztEa7bL)) to get the F1 season races (dates and times of all event: Practice, Qualifying, Race) and uses the [Google Calendar Api](https://developers.google.com/calendar/api) to add events in Google Calendar

- [Formula One Calendar](#formula-one-calendar)
  - [**Pre requirements**](#pre-requirements)
      - [Make your Calendar public and get Calendar Id](#make-your-calendar-public-and-get-calendar-id)
      - [Google Service Account](#google-service-account)
      - [Share your calendar with your Google Service Account Email](#share-your-calendar-with-your-google-service-account-email)
      - [.Net Core 3.1 or Docker](#net-core-31-or-docker)
  - [**How to Use**](#how-to-use)
      - [Docker](#docker)
      - [.Net Core 3.1 (Debug/Release)](#net-core-31-debugrelease)

## **Pre requirements**
#### Make your Calendar public and get Calendar Id
> [Click here](https://yabdab.zendesk.com/hc/en-us/articles/205945926-Find-Google-Calendar-ID) to learn how to let your calendar public and how to get calendar id

#### Google Service Account
> [Click here](https://support.google.com/a/answer/7378726?hl=en) to get learn how to create Google Service Account to fill some properties in [appsettings](src/Script.FormulaOneCalendar/appsettings.json)

#### Share your calendar with your Google Service Account Email
> [Click here](https://support.google.com/a/users/answer/37082?hl=en) to read how share your Google Calendar and get the ***client_email*** from your service account json

#### .Net Core 3.1 or Docker
> To run this app, you will need to have [.Net Core 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) or [Docker](https://www.docker.com/get-started) installed and configured on your machine

## **How to Use**
#### Docker
1. To use with Docker, you will need to fill the environments variables in [Dockerfile](Dockerfile)
    ```dockerfile
    ENV ApplicationName "application name (auto fill)"
    ENV ErgastApiUrl "ergast api url base (auto fill)"
    ENV GoogleCalendarId "google calendar id"
    ENV ServiceAccountEmail "google service account client_email"
    ENV ServiceAccountPrivateKey "google service account private_key"
    ```

2. Build docker image
    ```powershell
    docker build . -t imageName
    ```

3. Run docker image
    ```powershell
    docker run imageName
    ```

> ℹ️ Change above **imageName** to a name that you want

#### .Net Core 3.1 (Debug/Release)
1. To use with .net core, you will need to fill the informations in [appsettings file](src/Script.FormulaOneCalendar/appsettings.json)
    
    ```json
    "appSettings": {
        "applicationName": "application name (auto fill)",
        "ergastApi": {
            "urlBase": "ergast api url base (auto fill)"
        },
        "google": {
            "calendar": {
                "id": "google calendar id"
            },
            "serviceAccount": {
                "email": "google service account client_email",
                "privateKey": "google service account private_key"
            }
        }
    }
    ```

2. Run the project (src/Script.FormulaOneCalendar/Script.FormulaOneCalendar.csproj)