using System;
using System.Linq;
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


            var schedules = await raceService.GetScheduledRacesAsync(currentYear);

            foreach (var raceEvent in schedules)
            {
                var result = await calendarService.CreateFormulaOneEventAsync(settings.Google.Calendar.Id, raceEvent);
                var calendarRace = await storageService.GetRaceAsync(result.Id);

                if (!string.IsNullOrWhiteSpace(calendarRace.Key))
                {
                    await calendarService.UpdateFormulaOneEventAsync(settings.Google.Calendar.Id, result.Id, raceEvent);
                }

                await storageService.SaveRaceAsync(result.Id, raceEvent);
            }
        }
    }
}
