using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Interfaces
{
    public interface ICalendarService
    {
        Task<Event> CreateFormulaOneEventAsync(string calendarId, RaceDetail raceEvent);
        Task<IEnumerable<Event>> GetAllFormulaOneEventsAsync(string calendarId);
        Task<Event> GetFormulaOneEventAsync(string calendarId, RaceDetail raceEvent);
        Task RemoveFormulaOneEventAsync(string calendarId, string eventId);
        Task UpdateFormulaOneEventAsync(string calendarId, string eventId, RaceDetail raceEvent);
    }
}