using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Service.Interfaces;

namespace Script.FormulaOneCalendar.Service
{
    public class GoogleCalendarService : ICalendarService
    {
        private readonly EventsResource _eventService;
        
        public GoogleCalendarService(CalendarService calendarService)
        {
            _eventService = calendarService.Events;
        }

        public async Task<Event> CreateFormulaOneEventAsync(string calendarId, Race race)
        {
            var googleEventResource = new Event
            {
                Summary = race.RaceName,
                Location = race.Circuit.Location.Country,
                Start = new EventDateTime
                {
                    DateTime = new DateTime(race.Date.Year, race.Date.Month, race.Date.Day, race.Time.Hour, race.Time.Minute, race.Time.Second)
                },
                End = new EventDateTime
                {
                    DateTime = new DateTime(race.Date.Year, race.Date.Month, race.Date.Day, race.Time.Hour, race.Time.Minute, race.Time.Second).AddHours(2)
                }
            };

            var request = _eventService.Insert(googleEventResource, calendarId);
            return await RequestGoogleAsync(request, CancellationToken.None);
        }

        public async Task UpdateFormulaOneEventAsync(string calendarId, string eventId, Race race)
        {
            var googleEventResource = new Event
            {
                Start = new EventDateTime
                {
                    DateTime = new DateTime(race.Date.Year, race.Date.Month, race.Date.Day, race.Time.Hour, race.Time.Minute, race.Time.Second)
                },
                End = new EventDateTime
                {
                    DateTime = new DateTime(race.Date.Year, race.Date.Month, race.Date.Day, race.Time.Hour, race.Time.Minute, race.Time.Second).AddHours(2)
                }
            };
            
            var request = _eventService.Update(null, calendarId, eventId);

            await RequestGoogleAsync(request, CancellationToken.None);
        }

        private async Task<T> RequestGoogleAsync<T>(CalendarBaseServiceRequest<T> eventRequest, CancellationToken cancellationToken)
        {
            return await eventRequest.ExecuteAsync(cancellationToken);
        }
    }
}