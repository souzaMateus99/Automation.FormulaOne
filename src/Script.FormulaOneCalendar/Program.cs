using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Script.FormulaOneCalendar.Model.Constants;
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
        private const string NEW_LINE_ESCAPE_RAW = "\\n";
        private const int ADD_UPDATE_CALENDAR_FLOW_OPTION = 1;
        private const int REMOVE_EVENT_CALENDAR_FLOW_OPTION = 2;
        private const int BOTH_FLOW_OPTION = 99;

        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
        }

        private static async Task RunAsync(string[] args)
        {
            var flow = args.FirstOrDefault() is null ? SetFlow() : int.Parse(args.FirstOrDefault());

            if (flow != default)
            {
                var currentYear = DateTime.Now.Year;

                var env = Environment.GetEnvironmentVariable(EnvironmentVariablesConst.ASPNET_ENV_VARIABLE_NAME);
                var appsettings = $"{APP_SETTINGS_SECTION}.{JSON_EXTENSION_WITHOUT_DOT}";
                var environmentSettings = $"{APP_SETTINGS_SECTION}.{env}.{JSON_EXTENSION_WITHOUT_DOT}";

                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(appsettings, optional: false, reloadOnChange: true)
                    .AddJsonFile(environmentSettings, optional: true, reloadOnChange: true)
                    .Build();

                var settings = GetAppSettings(config);

                IRaceService raceService = new RaceService(
                    RestEaseFactory.CreateClient<IFormulaOneClient>(settings.FormulaOne.UrlBase, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    settings
                );
                ICalendarService calendarService = new GoogleCalendarService(GoogleCalendarFactory.CreateWithServiceAccount(settings));
                IStorageService storageService = new FileStorageService(currentYear);

                if (flow.Equals(ADD_UPDATE_CALENDAR_FLOW_OPTION) || flow.Equals(BOTH_FLOW_OPTION))
                {
                    await AddAndUpdateF1SeasonAsync(settings, currentYear, raceService, calendarService, storageService);
                }

                if (flow.Equals(REMOVE_EVENT_CALENDAR_FLOW_OPTION) || flow.Equals(BOTH_FLOW_OPTION))
                {
                    await RemoveF1SeasonAsync(settings, calendarService, storageService);
                }
            }
        }

        private static int SetFlow()
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("1- Add/Update F1 event to calendar");
            Console.WriteLine("2- Remove F1 event to calendar");
            Console.WriteLine("99- Both above options");

            var userInput = Console.ReadLine();

            if (int.TryParse(userInput, out var value))
            {
                return value;
            }

            return default;
        }

        private static AppSettings GetAppSettings(IConfiguration configuration)
        {
            if (configuration.GetSection(APP_SETTINGS_SECTION).Exists())
            {
                return configuration.GetSection(APP_SETTINGS_SECTION).Get<AppSettings>();
            }

            return new AppSettings
            {
                ApplicationName = Environment.GetEnvironmentVariable(EnvironmentVariablesConst.APPLICATION_NAME_ENV),
                FormulaOne = new FormulaOneSettings
                {
                    UrlBase = new Uri(Environment.GetEnvironmentVariable(EnvironmentVariablesConst.ERGAST_API_URL_ENV))
                },
                Google = new GoogleSettings
                {
                    Calendar = new CalendarSettings
                    {
                        Id = Environment.GetEnvironmentVariable(EnvironmentVariablesConst.GOOGLE_CALENDAR_ID_ENV)
                    },
                    ServiceAccount = new ServiceAccountSettings
                    {
                        Email = Environment.GetEnvironmentVariable(EnvironmentVariablesConst.SERVICE_ACCOUNT_EMAIL_ENV),
                        PrivateKey = Environment.GetEnvironmentVariable(EnvironmentVariablesConst.SERVICE_ACCOUNT_PRIVATE_KEY_ENV).Contains(NEW_LINE_ESCAPE_RAW)
                                        ? Environment.GetEnvironmentVariable(EnvironmentVariablesConst.SERVICE_ACCOUNT_PRIVATE_KEY_ENV).Replace(NEW_LINE_ESCAPE_RAW, string.Empty)
                                        : Environment.GetEnvironmentVariable(EnvironmentVariablesConst.SERVICE_ACCOUNT_PRIVATE_KEY_ENV)
                    }
                }
            };
        }

        private static async Task AddAndUpdateF1SeasonAsync(AppSettings settings, int year, IRaceService raceService, ICalendarService calendarService, IStorageService storageService)
        {
            var seasonRaces = await raceService.GetScheduledRacesAsync(year);

            foreach (var race in seasonRaces)
            {
                var isGetDetail = await raceService.GetRaceDetailsAsync(race);

                if (isGetDetail)
                {
                    foreach (var raceDetail in race.RaceDetails)
                    {
                        var calendarEvent = await calendarService.GetFormulaOneEventAsync(settings.Google.Calendar.Id, raceDetail);

                        if (calendarEvent is null)
                        {
                            var result = await calendarService.CreateFormulaOneEventAsync(settings.Google.Calendar.Id, raceDetail);

                            calendarEvent = result;
                        }
                        else
                        {
                            var calendarRace = await storageService.GetRaceAsync(calendarEvent.Id);

                            if (!string.IsNullOrWhiteSpace(calendarRace.Key))
                            {
                                await calendarService.UpdateFormulaOneEventAsync(settings.Google.Calendar.Id, calendarEvent.Id, raceDetail);
                            }
                        }

                        await storageService.SaveRaceAsync(calendarEvent.Id, race);
                    }
                }
            }
        }

        private static async Task RemoveF1SeasonAsync(AppSettings settings, ICalendarService calendarService, IStorageService storageService)
        {
            var calendarRaces = await calendarService.GetAllFormulaOneEventsAsync(settings.Google.Calendar.Id);

            foreach (var calendarRace in calendarRaces)
            {
                var storageRace = await storageService.GetRaceAsync(calendarRace.Id);

                if (!string.IsNullOrWhiteSpace(storageRace.Key))
                {
                    await calendarService.RemoveFormulaOneEventAsync(settings.Google.Calendar.Id, calendarRace.Id);
                }
            }
        }
    }
}
