using System.Threading.Tasks;
using RestEase;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Clients
{
    public interface IErgastClient
    {
        [Get("{year}")]
        Task<RaceSchedule> GetScheduledRacesAsync([Path("year")] int year);
    }
}