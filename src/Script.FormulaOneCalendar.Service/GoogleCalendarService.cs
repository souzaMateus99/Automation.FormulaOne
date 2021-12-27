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

        public async Task<Event> CreateFormulaOneEventAsync(string calendarId, RaceSchedule race)
        {
            var googleEventResource = new Event
            {
                Summary = race.RaceName,
                Location = race.Circuit.Locality,
                Start = new EventDateTime
                {
                    DateTime = DateTime.Parse("")
                },
                End = new EventDateTime
                {
                    DateTime = DateTime.Parse("")
                }
            };

            var request = _eventService.Insert(googleEventResource, calendarId);
            return await RequestGoogleAsync(request, CancellationToken.None);
        }

        private async Task<T> RequestGoogleAsync<T>(CalendarBaseServiceRequest<T> eventRequest, CancellationToken cancellationToken)
        {
            return await eventRequest.ExecuteAsync(cancellationToken);
        }
    }
}