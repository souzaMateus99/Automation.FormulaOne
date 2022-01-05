using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class ServiceAccountSettings
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }
    }
}