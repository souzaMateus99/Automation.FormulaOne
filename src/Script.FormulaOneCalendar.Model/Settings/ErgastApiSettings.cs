using System;
using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.Settings
{
    public class ErgastApiSettings
    {
        [JsonProperty("urlBase")]
        public Uri UrlBase { get; set; }
    }
}