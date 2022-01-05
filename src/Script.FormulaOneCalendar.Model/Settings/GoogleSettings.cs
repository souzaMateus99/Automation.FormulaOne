using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class GoogleSettings
    {
        [JsonProperty("calendar")]
        public CalendarSettings Calendar { get; set; }

        [JsonProperty("serviceAccount")]
        public ServiceAccountSettings ServiceAccount { get; set; }
    }
}