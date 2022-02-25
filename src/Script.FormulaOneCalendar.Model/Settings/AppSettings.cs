using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class AppSettings
    {
        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty("formulaOne")]
        public FormulaOneSettings FormulaOne { get; set; }

        [JsonProperty("google")]
        public GoogleSettings Google { get; set; }
    }
}