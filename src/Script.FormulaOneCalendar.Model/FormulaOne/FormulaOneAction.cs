using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneAction
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("targetType")]
        public string TargetType { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}