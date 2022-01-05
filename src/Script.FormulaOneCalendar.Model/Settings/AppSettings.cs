using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class AppSettings
    {
        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty("ergastApi")]
        public ErgastApiSettings ErgastApi { get; set; }

        [JsonProperty("google")]
        public GoogleSettings Google { get; set; }
    }
}