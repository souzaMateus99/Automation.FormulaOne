using System.Collections.Generic;
using System.Threading.Tasks;
using RestEase;
using Script.FormulaOneCalendar.Model;

namespace Script.FormulaOneCalendar.Service.Clients
{
    public interface IErgastClient
    {
        [Get("{year}")]
        Task<Season> GetScheduledRacesAsync([Path("year")] int year);
    }
}