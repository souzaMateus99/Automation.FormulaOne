using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Interfaces
{
    public interface ICalendarService
    {
        Task<Event> CreateFormulaOneEventAsync(string calendarId, Race race);
        Task<Event> GetFormulaOneEventAsync(string calendarId, Race race);
        Task UpdateFormulaOneEventAsync(string calendarId, string eventId, Race race);
    }
}