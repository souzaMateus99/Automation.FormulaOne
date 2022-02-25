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

        public async Task<Event> CreateFormulaOneEventAsync(string calendarId, RaceDetail raceEvent)
        {
            var googleEventResource = GetEvent(raceEvent.Title, raceEvent.StartDate, raceEvent.EndDate);
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

        public async Task<Event> GetFormulaOneEventAsync(string calendarId, RaceDetail raceEvent)
        {
            var races = await GetAllFormulaOneEventsAsync(calendarId);

            return races.FirstOrDefault(r => r.Summary.Equals(raceEvent.Title)
                                        && r.Start.DateTime.Value.Date.Equals(raceEvent.StartDate.Date));
        }

        public async Task RemoveFormulaOneEventAsync(string calendarId, string eventId)
        {
            var request = _eventService.Delete(calendarId, eventId);

            var text = await RequestGoogleAsync(request);
        }

        public async Task UpdateFormulaOneEventAsync(string calendarId, string eventId, RaceDetail raceEvent)
        {
            var googleEventResource = GetEvent(raceEvent.Title, raceEvent.StartDate, raceEvent.EndDate);

            var request = _eventService.Update(googleEventResource, calendarId, eventId);

            await RequestGoogleAsync(request);
        }

        private Event GetEvent(string title, DateTime dateStart, DateTime dateFinish)
        {
            return new Event
            {
                Summary = title,
                Description = EVENT_DESCRIPTION_TEXT,
                Start = new EventDateTime
                {
                    DateTime = dateStart
                },
                End = new EventDateTime
                {
                    DateTime = dateFinish
                }
            };
        }

        private async Task<T> RequestGoogleAsync<T>(CalendarBaseServiceRequest<T> eventRequest)
        {
            return await eventRequest.ExecuteAsync();
        }
    }
}