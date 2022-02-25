using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class FormulaOneYearsPage
    {
        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("pageId")]
        public string PageId { get; set; }
    }
}