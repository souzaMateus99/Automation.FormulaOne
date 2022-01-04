using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Script.FormulaOneCalendar.Service.Factories
{
    public static class GoogleCalendarFactory
    {
        private const string APPLICATION_NAME_VALUE = "FormulaOneCalendar";

        private static ServiceAccountCredential GetServiceAccountCredential(string filepath)
        {
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                return ServiceAccountCredential.FromServiceAccountData(stream);
            }
        }
        
        public static CalendarService CreateWithServiceAccount(string clientSecretsPath)
        {
            var credential = GetServiceAccountCredential(clientSecretsPath);

            return new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME_VALUE
            });
        }
    }
}