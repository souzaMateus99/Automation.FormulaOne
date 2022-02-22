using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneRetrieveItems
    {
        [JsonProperty("resultObj")]
        public FormulaOneResultObject ResultObject { get; set; }

        [JsonProperty("uriOriginal")]
        public string UriOriginal { get; set; }

        [JsonProperty("typeOriginal")]
        public string TypeOriginal { get; set; }
    }
}