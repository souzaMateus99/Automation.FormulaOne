using System.Collections.Generic;
using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneResultObject
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("containers")]
        public IEnumerable<FormulaOneContainer> Containers { get; set; }

        [JsonProperty("metadata")]
        public FormulaOneMetadata Metadata { get; set; }
    }
}