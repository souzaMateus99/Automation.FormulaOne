using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Service.Clients;
using Script.FormulaOneCalendar.Service.Interfaces;
using Script.FormulaOneCalendar.Model.Settings;
using Script.FormulaOneCalendar.Model.FormulaOne;
using System.Net;

namespace Script.FormulaOneCalendar.Service
{
    public class RaceService : IRaceService
    {
        private readonly IFormulaOneClient _formulaOneClient;
        private readonly FormulaOneSettings _formulaOneSettings;
        private readonly DateTime _initialUnixDate;

        private const string BROWSER_USER_AGENT_VALUE = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36 Edg/98.0.1108.56";
        private const int UNIX_INIT_YEAR = 1970;
        private const int ONE_VALUE = 1;
        private const int ZERO_VALUE = 0;

        public RaceService(IFormulaOneClient formulaOneClient, AppSettings appSettings)
        {
            formulaOneClient.UserAgent = BROWSER_USER_AGENT_VALUE;
            _formulaOneClient = formulaOneClient;
            _formulaOneSettings = appSettings.FormulaOne;
            _initialUnixDate = new DateTime(UNIX_INIT_YEAR, ONE_VALUE, ONE_VALUE, ZERO_VALUE, ZERO_VALUE, ZERO_VALUE, DateTimeKind.Utc);
        }

        public async Task<IEnumerable<Race>> GetScheduledRacesAsync(int year)
        {
            var formulaOneYearPage = _formulaOneSettings.YearsPageId.FirstOrDefault(y => y.Year.Equals(year));

            if (formulaOneYearPage != null)
            {
                var pageResponse = await RequestFormulaOneClientAsync(formulaOneYearPage.PageId);

                if (IsValidPageResponse(pageResponse) && IsValidResultObject(pageResponse.ResultObject))
                {
                    var seasonRaces = pageResponse.ResultObject.Containers
                        .Where(c => c.Layout.Equals("vertical_thumbnail"))
                        .Select(c => c)
                        .Where(c => IsValidResultObject(c.RetrieveItems.ResultObject))
                        .SelectMany(c => c.RetrieveItems.ResultObject.Containers)
                        .Where(c => c.Metadata.Genres.Contains("RACE") && !string.IsNullOrWhiteSpace(c.Metadata.Attributes.PageId));

                    var races = seasonRaces.Select(race => new Race
                    {
                        RaceName = race.Metadata.Attributes.MeetingOfficialName,
                        Circuit = race.Metadata.Attributes.CircuitShortName,
                        StartDate = race.Metadata.Attributes.MeetingStartDate.HasValue ? race.Metadata.Attributes.MeetingStartDate.Value.ToUniversalTime() : new DateTime(),
                        EndDate = race.Metadata.Attributes.MeetingEndDate.HasValue ? race.Metadata.Attributes.MeetingEndDate.Value.ToUniversalTime() : new DateTime(),
                        Season = race.Metadata.Season,
                        Round = int.TryParse(race.Metadata.Attributes.MeetingNumber, out var round) ? round : 0,
                        Country = race.Metadata.Attributes.Country,
                        Location = race.Metadata.Attributes.MeetingLocation,
                        PageId = race.Metadata.Attributes.PageId
                    });

                    return races;
                }
            }

            return Enumerable.Empty<Race>();
        }

        public async Task<bool> GetRaceDetailsAsync(Race race)
        {
            var pageResponse = await RequestFormulaOneClientAsync(race.PageId);
            var isValidResponse = IsValidPageResponse(pageResponse) && IsValidResultObject(pageResponse?.ResultObject);

            if (isValidResponse)
            {
                var raceDetails = pageResponse.ResultObject.Containers
                    .Where(c => IsValidResultObject(c.RetrieveItems.ResultObject))
                    .SelectMany(c => c.RetrieveItems.ResultObject.Containers)
                    .Where(c => c.Events != null && !string.IsNullOrWhiteSpace(c.EventName));

                var formulaOneRaceDetail = raceDetails.FirstOrDefault(r => r.EventName.Equals("Formula 1", StringComparison.OrdinalIgnoreCase));

                if (formulaOneRaceDetail != null)
                {
                    var details = formulaOneRaceDetail.Events.Select(d => new RaceDetail
                    {
                        Title = d.Metadata.Attributes.GlobalTitle,
                        Type = d.Metadata.Genres.FirstOrDefault(),
                        Description = d.Metadata.LongDescription,
                        StartDate = _initialUnixDate.AddMilliseconds(d.Metadata.Attributes.SessionStartDate).ToLocalTime(),
                        EndDate = _initialUnixDate.AddMilliseconds(d.Metadata.Attributes.SessionEndDate).ToLocalTime()
                    });

                    foreach (var detail in details)
                    {
                        race.RaceDetails.Add(detail);
                    }

                    return true;
                }
            }

            return false;
        }

        private async Task<FormulaOnePageResponse> RequestFormulaOneClientAsync(string pageId)
        {
            return await _formulaOneClient.GetPageInfoAsync(_formulaOneSettings.ApiVersion, _formulaOneSettings.Language, pageId);
        }

        private bool IsValidPageResponse(FormulaOnePageResponse pageResponse)
        {
            return pageResponse != null
            && pageResponse.ResultCode.Equals(nameof(HttpStatusCode.OK), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidResultObject(FormulaOneResultObject resultObject)
        {
            return resultObject != null
            && resultObject.Total > 0;
        }
    }
}
