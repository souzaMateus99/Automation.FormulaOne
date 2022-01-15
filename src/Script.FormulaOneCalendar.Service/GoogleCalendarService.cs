using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Polly;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Service.Interfaces;

namespace Script.FormulaOneCalendar.Service
{
    public class GoogleCalendarService : ICalendarService
    {
        private const int TOTAL_RETRY_ATTEMPTS_VALUE = 5;
        
        private readonly EventsResource _eventService;
        
        public GoogleCalendarService(CalendarService calendarService)
        {
            _eventService = calendarService.Events;
        }

        public async Task<Event> CreateFormulaOneEventAsync(string calendarId, Race race)
        {
            var googleEventResource = GetEvent(race);

            var request = _eventService.Insert(googleEventResource, calendarId);
            return await RequestGoogleAsync(request);
        }

        public async Task<Event> GetFormulaOneEventAsync(string calendarId, Race race)
        {
            var request = _eventService.List(calendarId);
            
            var raceFiltered = await Policy
                .HandleResult<Event>(e => e is null)
                .RetryAsync(TOTAL_RETRY_ATTEMPTS_VALUE)
                .ExecuteAsync(async () => 
                {
                    var events = await RequestGoogleAsync(request);
                    request.PageToken = events.NextPageToken;

                    return events.Items
                        .FirstOrDefault(i => i.Summary.Equals(race.RaceName) && i.Start.DateTime.Value.Date.Equals(race.Date.Date));
                });

            return raceFiltered;
        }

        public async Task UpdateFormulaOneEventAsync(string calendarId, string eventId, Race race)
        {
            var googleEventResource = GetEvent(race);
            
            var request = _eventService.Update(googleEventResource, calendarId, eventId);

            await RequestGoogleAsync(request);
        }

        private Event GetEvent(Race race)
        {
            return new Event
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
        }

        private async Task<T> RequestGoogleAsync<T>(CalendarBaseServiceRequest<T> eventRequest)
        {
            return await eventRequest.ExecuteAsync();
        }
    }
}