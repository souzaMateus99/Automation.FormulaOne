using System;
using System.Threading.Tasks;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Service.Clients;
using Script.FormulaOneCalendar.Service.Interfaces;

namespace Script.FormulaOneCalendar.Service
{
    public class RaceService : IRaceService
    {
        private IErgastClient _ergastClient;
        
        public RaceService(IErgastClient ergastClient)
        {
            _ergastClient = ergastClient;
        }
        
        public async Task<RaceSchedule> GetScheduledRacesAsync(int year)
        {
            return await _ergastClient.GetScheduledRacesAsync(year);
        }
    }
}
