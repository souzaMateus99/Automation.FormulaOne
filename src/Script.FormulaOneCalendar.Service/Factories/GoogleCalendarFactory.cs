using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Script.FormulaOneCalendar.Model.Settings;

namespace Script.FormulaOneCalendar.Service.Factories
{
    public static class GoogleCalendarFactory
    {
        private static string[] _scopes = { CalendarService.ScopeConstants.CalendarEventsReadonly, CalendarService.ScopeConstants.CalendarEvents };

        private static ServiceAccountCredential GetServiceAccountCredential(ServiceAccountSettings serviceAccountSettings)
        {
                return new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountSettings.Email)
                {
                    Scopes = _scopes
                }.FromPrivateKey(serviceAccountSettings.PrivateKey));
        }
        
        public static CalendarService CreateWithServiceAccount(AppSettings appSettings)
        {
            var credential = GetServiceAccountCredential(appSettings.Google.ServiceAccount);

            return new CalendarService(new BaseClientService.Initializer()
            {
                ApplicationName = appSettings.ApplicationName,
                HttpClientInitializer = credential
            });
        }
    }
}