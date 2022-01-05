using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class CalendarSettings
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}