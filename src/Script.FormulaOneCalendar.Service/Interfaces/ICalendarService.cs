using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Interfaces
{
    public interface ICalendarService
    {
        Task<Event> CreateFormulaOneEventAsync(string calendarId, RaceSchedule race);
    }
}