using System.Collections.Generic;
using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneTranslations
    {
        [JsonProperty("metadata.label")]
        public Dictionary<string, Dictionary<string, string>> MetadataLabel { get; set; }
    }
}