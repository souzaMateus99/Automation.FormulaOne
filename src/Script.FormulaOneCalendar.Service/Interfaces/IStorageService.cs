using System.Collections.Generic;
using System.Threading.Tasks;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Interfaces
{
    public interface IStorageService
    {
        Task SaveRaceAsync(string eventId, Race race);
        Task<KeyValuePair<string, Race>> GetRaceAsync(string eventId);
    }
}