using System.Collections.Generic;
using System.Threading.Tasks;
using RestEase;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Model.FormulaOne;

namespace Script.FormulaOneCalendar.Service.Clients
{
    public interface IFormulaOneClient : IFormulaOneHeaders
    {
        [Get("{apiVersion}/A/{language}/WEB_DASH/ALL/PAGE/{pageId}/Anonymous/2")]
        Task<FormulaOnePageResponse> GetPageInfoAsync([Path("apiVersion")] string apiVersion, [Path("language")] string language, [Path("pageId")] string pageId);
    }
}