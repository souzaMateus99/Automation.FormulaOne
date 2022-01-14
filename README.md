This project is an automation that add and update the F1 season in a Google Calendar.

> ⚠️ [Click here](https://calendar.google.com/calendar/u/0?cid=ZjdycmM5b2V1bmVhbWhpN2dnMzlic29kczBAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ) if you want the F1 season without run this app. 
> <p>The link will add you in a public calendar with the F1 races</p>

## Pre requirements
#### **Make your Calendar public and get Calendar Id**
> [Click here](https://yabdab.zendesk.com/hc/en-us/articles/205945926-Find-Google-Calendar-ID) to learn how to let your calendar public and how to get calendar id

#### **Google Service Account**
> [Click here](https://support.google.com/a/answer/7378726?hl=en) to get learn how to create Google Service Account to fill some properties in [appsettings](src/Script.FormulaOneCalendar/appsettings.json)

#### **Share your calendar with your Google Service Account Email**
> [Click here](https://support.google.com/a/users/answer/37082?hl=en) to read how share your Google Calendar and get the ***client_email*** from your service account json

#### **.Net Core 3.1 installed on machine**
> To run this app, you will need to have [.Net Core 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) installed on your machine

## How to Use
To use, you will need to fill the informations in [appsettings file](src/Script.FormulaOneCalendar/appsettings.json)