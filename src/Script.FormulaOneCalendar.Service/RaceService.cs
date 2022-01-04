using System;
using System.Collections.Generic;
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
        
        public async Task<IEnumerable<Race>> GetScheduledRacesAsync(int year)
        {
            var season = await _ergastClient.GetScheduledRacesAsync(year);

            return season.RacesInfo.Races;
        }
    }
}
