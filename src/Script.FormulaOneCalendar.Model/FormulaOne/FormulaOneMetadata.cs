using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneMetadata
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("plainText")]
        public string PlainText { get; set; }

        [JsonProperty("htmlText")]
        public string HtmlText { get; set; }

        [JsonProperty("emfAttributes")]
        public FormulaOneAttributes Attributes { get; set; }

        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }

        [JsonProperty("genres")]
        public string[] Genres { get; set; }

        [JsonProperty("season")]
        public long Season { get; set; }
    }
}