using System.Threading.Tasks;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Interfaces
{
    public interface IRaceService
    {
        Task<RaceSchedule> GetScheduledRacesAsync(int year);
    }
}