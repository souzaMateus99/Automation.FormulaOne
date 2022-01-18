using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Model.Settings;
using Script.FormulaOneCalendar.Service;
using Script.FormulaOneCalendar.Service.Clients;
using Script.FormulaOneCalendar.Service.Factories;
using Script.FormulaOneCalendar.Service.Interfaces;

namespace Script.FormulaOneCalendar
{
    class Program
    {
        private const string APP_SETTINGS_SECTION = "appsettings";
        private const string JSON_EXTENSION_WITHOUT_DOT = "json";
        private const string ASPNET_ENV_VARIABLE_NAME = "ASPNETCORE_ENVIRONMENT";

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var currentYear = DateTime.Now.Year;
            
            var env = Environment.GetEnvironmentVariable(ASPNET_ENV_VARIABLE_NAME);
            var appsettings = $"{APP_SETTINGS_SECTION}.{JSON_EXTENSION_WITHOUT_DOT}";
            var environmentSettings = $"{APP_SETTINGS_SECTION}.{env}.{JSON_EXTENSION_WITHOUT_DOT}";

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(appsettings, optional: false,  reloadOnChange: true)
                .AddJsonFile(environmentSettings, optional: true)
                .Build();

            var settings = config.GetSection(APP_SETTINGS_SECTION).Get<AppSettings>();
            
            IRaceService raceService = new RaceService(RestEaseFactory.CreateClientWithXmlResponse<IErgastClient>(settings.ErgastApi.UrlBase.ToString()));
            ICalendarService calendarService = new GoogleCalendarService(GoogleCalendarFactory.CreateWithServiceAccount(settings));
            IStorageService storageService = new FileStorageService(currentYear);

            await AddAndUpdateF1SeasonAsync(settings, currentYear, raceService, calendarService, storageService);
            await RemoveF1SeasonAsync(settings, calendarService, storageService);
        }

        private static async Task AddAndUpdateF1SeasonAsync(AppSettings settings, int year, IRaceService raceService, ICalendarService calendarService, IStorageService storageService)
        {
            var schedules = await raceService.GetScheduledRacesAsync(year);
            
            foreach (var raceEvent in schedules)
            {
                var calendarEvent = await calendarService.GetFormulaOneEventAsync(settings.Google.Calendar.Id, raceEvent);

                if (calendarEvent is null)
                {
                    var result = await calendarService.CreateFormulaOneEventAsync(settings.Google.Calendar.Id, raceEvent);

                    calendarEvent = result;
                }
                else
                {
                    var calendarRace = await storageService.GetRaceAsync(calendarEvent.Id);

                    if (!string.IsNullOrWhiteSpace(calendarRace.Key))
                    {
                        await calendarService.UpdateFormulaOneEventAsync(settings.Google.Calendar.Id, calendarEvent.Id, raceEvent);
                    }
                }

                await storageService.SaveRaceAsync(calendarEvent.Id, raceEvent);
            }
        }

        private static async Task RemoveF1SeasonAsync(AppSettings settings, ICalendarService calendarService, IStorageService storageService)
        {
            var calendarRaces = await calendarService.GetAllFormulaOneEventsAsync(settings.Google.Calendar.Id);
            
            foreach (var calendarRace in calendarRaces)
            {
                var storageRace = await storageService.GetRaceAsync(calendarRace.Id);

                if (string.IsNullOrWhiteSpace(storageRace.Key))
                {
                    await calendarService.RemoveFormulaOneEventAsync(settings.Google.Calendar.Id, calendarRace.Id);
                }
            }
        }
    }
}
