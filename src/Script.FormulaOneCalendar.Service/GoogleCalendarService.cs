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
        private const string EVENT_DESCRIPTION_TEXT = "Event created by \"F1 - calendar\" automation";
        
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

        public async Task<IEnumerable<Event>> GetAllFormulaOneEventsAsync(string calendarId)
        {
            var request = _eventService.List(calendarId);
            var totalRetryAttempt = 1;
            var items = new List<Event>();

            await Policy
                .HandleResult<Events>(e => e is null || !string.IsNullOrWhiteSpace(e.NextPageToken))
                .RetryAsync(totalRetryAttempt, onRetry: (eventResult, retry) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventResult.Result.NextPageToken))
                    {
                        totalRetryAttempt++;
                    }
                })
                .ExecuteAsync(async () => 
                {
                    var events = await RequestGoogleAsync(request);
                    request.PageToken = events.NextPageToken;

                    var raceEvents = events.Items
                                        .Where(e => e.Start.DateTime.Value.Date >= DateTime.Now.Date && e.Description.Equals(EVENT_DESCRIPTION_TEXT));

                    items.AddRange(raceEvents);

                    return events;
                });

            return items;
        }

        public async Task<Event> GetFormulaOneEventAsync(string calendarId, Race race)
        {
            var races = await GetAllFormulaOneEventsAsync(calendarId);

            return races.FirstOrDefault(r => r.Summary.Equals(race.RaceName)
                                        && r.Location.Equals(race.Circuit.Location.Country)
                                        && r.Start.DateTime.Value.Date.Equals(race.Date.Date));
        }

        public async Task RemoveFormulaOneEventAsync(string calendarId, string eventId)
        {
            var request = _eventService.Delete(calendarId, eventId);

            var text = await RequestGoogleAsync(request);
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
                Description = EVENT_DESCRIPTION_TEXT,
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