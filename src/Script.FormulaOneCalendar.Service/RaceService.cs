using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Service.Clients;
using Script.FormulaOneCalendar.Service.Interfaces;
using Script.FormulaOneCalendar.Model.Settings;
using Script.FormulaOneCalendar.Model.FormulaOne;

namespace Script.FormulaOneCalendar.Service
{
    public class RaceService : IRaceService
    {
        private readonly IFormulaOneClient _formulaOneClient;
        private readonly FormulaOneSettings _formulaOneSettings;

        private const string BROWSER_USER_AGENT_VALUE = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36 Edg/98.0.1108.56";

        public RaceService(IFormulaOneClient formulaOneClient, AppSettings appSettings)
        {
            formulaOneClient.UserAgent = BROWSER_USER_AGENT_VALUE;
            _formulaOneClient = formulaOneClient;
            _formulaOneSettings = appSettings.FormulaOne;
        }

        public async Task<IEnumerable<Race>> GetScheduledRacesAsync(int year)
        {
            var formulaOneYearPage = _formulaOneSettings.YearsPageId.FirstOrDefault(y => y.Year.Equals(year));

            if (formulaOneYearPage != null)
            {
                var pageResponse = await RequestFormulaOneClientAsync(formulaOneYearPage.PageId);

                if (IsValidResultObject(pageResponse.ResultObject))
                {
                    var seasonRaces = pageResponse.ResultObject.Containers
                        .Where(c => c.Layout.Equals("vertical_thumbnail"))
                        .Select(c => c)
                        .Where(c => IsValidResultObject(c.RetrieveItems.ResultObject))
                        .SelectMany(c => c.RetrieveItems.ResultObject.Containers)
                        .Where(c => c.Metadata.Genres.Contains("RACE"));
                    
                    return seasonRaces.Select(race => new Race
                        {
                            RaceName = race.Metadata.Attributes.MeetingOfficialName,
                            Circuit = race.Metadata.Attributes.CircuitShortName,
                            StartDate = race.Metadata.Attributes.MeetingStartDate.HasValue ? race.Metadata.Attributes.MeetingStartDate.Value.ToUniversalTime() : new DateTime(),
                            EndDate = race.Metadata.Attributes.MeetingEndDate.HasValue ? race.Metadata.Attributes.MeetingEndDate.Value.ToUniversalTime() : new DateTime(),
                            Season = race.Metadata.Season,
                            Round = int.TryParse(race.Metadata.Attributes.MeetingNumber, out var round) ? round : 0,
                            PageId = race.Metadata.Attributes.PageId
                        });
                }
            }

            return Enumerable.Empty<Race>();
        }
        
        public async Task<bool> GetRacesDetailAsync(IEnumerable<Race> races)
        {
            foreach (var race in races)
            {
                var pageResponse = await RequestFormulaOneClientAsync(race.PageId);
            }

            return true;
        }

        private async Task<FormulaOnePageResponse> RequestFormulaOneClientAsync(string pageId)
        {
            return await _formulaOneClient.GetPageInfoAsync(_formulaOneSettings.ApiVersion, _formulaOneSettings.Language, pageId);
        }

        private bool IsValidResultObject(FormulaOneResultObject resultObject)
        {
            return resultObject != null && resultObject.Total > 0;
        }
    }
}
