using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOnePageResponse
    {
        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }

        [JsonProperty("resultObj")]
        public FormulaOneResultObject ResultObject { get; set; }
    }
}