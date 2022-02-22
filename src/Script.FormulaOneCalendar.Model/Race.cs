using System;
using System.Xml.Serialization;

namespace Script.FormulaOneCalendar.Model
{
    public class Race
    {
        public string RaceName { get; set; }
        public string Circuit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long Season { get; set; }
        public int Round { get; set; }
        public string PageId { get; set; }
    }
}