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
        private const string APP_SETTINGS_FILENAME = "appsettings{0}.json";
        private const string APP_SETTINGS_SECTION = "appsettings";

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var currentYear = DateTime.Now.Year;
            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appsettings = string.Format(APP_SETTINGS_FILENAME, string.Empty);
            var environmentSettings = string.Format(APP_SETTINGS_FILENAME, $".{env}");

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(appsettings, optional: false,  reloadOnChange: true)
                .AddJsonFile(environmentSettings, optional: true)
                .Build();

            var settings = config.GetSection(APP_SETTINGS_SECTION).Get<AppSettings>();
            
            IRaceService raceService = new RaceService(RestEaseFactory.CreateClientWithXmlResponse<IErgastClient>(settings.ErgastApi.UrlBase.ToString()));
            ICalendarService calendarService = new GoogleCalendarService(GoogleCalendarFactory.CreateWithServiceAccount(settings));

            var schedules = await raceService.GetScheduledRacesAsync(currentYear);
            var firstRace = schedules.FirstOrDefault();

            var result = await calendarService.CreateFormulaOneEventAsync(settings.Google.Calendar.Id, firstRace);

            if (!string.IsNullOrWhiteSpace(result.Id))
            {
                IStorageService storageService = new FileStorageService(currentYear);
                await storageService.SaveRaceAsync(result.Id, firstRace);
            }
        }
    }
}
