using System.Collections.Generic;
using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneContainer
    {
        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("metadata")]
        public FormulaOneMetadata Metadata { get; set; }

        [JsonProperty("retrieveItems")]
        public FormulaOneRetrieveItems RetrieveItems { get; set; }

        [JsonProperty("actions")]
        public IEnumerable<FormulaOneAction> Actions { get; set; }

        // [JsonProperty("translations")]
        // public FormulaOneTranslations Translations { get; set; }
    }
}