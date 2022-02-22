using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class FormulaOneSettings
    {
        [JsonProperty("urlBase")]
        public Uri UrlBase { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("yearsPageId")]
        public FormulaOneYearsPage[] YearsPageId { get; set; }
    }
}