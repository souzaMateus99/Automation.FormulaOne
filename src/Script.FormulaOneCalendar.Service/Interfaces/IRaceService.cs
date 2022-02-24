using System.Collections.Generic;
using System.Threading.Tasks;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Interfaces
{
    public interface IRaceService
    {
        Task<IEnumerable<Race>> GetScheduledRacesAsync(int year);
        Task<bool> GetRaceDetailsAsync(Race race);
    }
}