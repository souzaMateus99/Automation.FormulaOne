using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Script.FormulaOneCalendar.Service.Factories
{
    public static class GoogleCalendarFactory
    {
        private static string[] _scopes = { CalendarService.Scope.CalendarReadonly, CalendarService.Scope.CalendarEvents };
        
        private const string APPLICATION_NAME = "FormulaOneCalendar";
        
        public static CalendarService CreateClientWithSecrets(string clientSecretsPath, string userName = "")
        {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromFile(clientSecretsPath).Secrets,
                _scopes,
                userName,
                CancellationToken.None
            ).GetAwaiter().GetResult();

            return new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }
    }
}