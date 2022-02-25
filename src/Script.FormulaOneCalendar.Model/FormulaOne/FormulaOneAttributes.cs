using System;
using Newtonsoft.Json;

namespace Script.FormulaOneCalendar.Model.FormulaOne
{
    public class FormulaOneAttributes
    {
        [JsonProperty("Meeting_Name")]
        public string MeetingName { get; set; }

        [JsonProperty("Meeting_Number")]
        public string MeetingNumber { get; set; }

        [JsonProperty("Circuit_Short_Name")]
        public string CircuitShortName { get; set; }

        [JsonProperty("Meeting_Code")]
        public string MeetingCode { get; set; }

        [JsonProperty("MeetingCountryKey")]
        public string MeetingCountryKey { get; set; }

        [JsonProperty("CircuitKey")]
        public string CircuitKey { get; set; }

        [JsonProperty("Meeting_Location")]
        public string MeetingLocation { get; set; }

        [JsonProperty("Circuit_Official_Name")]
        public string CircuitOfficialName { get; set; }

        [JsonProperty("Meeting_Start_Date")]
        public DateTime? MeetingStartDate { get; set; }

        [JsonProperty("Meeting_End_Date")]
        public DateTime? MeetingEndDate { get; set; }

        [JsonProperty("Meeting_Official_Name")]
        public string MeetingOfficialName { get; set; }

        [JsonProperty("Meeting_Display_Date")]
        public string MeetingDisplayDate { get; set; }

        [JsonProperty("Global_Title")]
        public string GlobalTitle { get; set; }

        [JsonProperty("PageID")]
        public string PageId { get; set; }

        [JsonProperty("sessionStartDate")]
        public long SessionStartDate { get; set; }

        [JsonProperty("sessionEndDate")]
        public long SessionEndDate { get; set; }

        [JsonProperty("Global_Meeting_Country_Name")]
        public string Country { get; set; }
    }
}