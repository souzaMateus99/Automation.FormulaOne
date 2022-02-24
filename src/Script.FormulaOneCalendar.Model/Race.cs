using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Script.FormulaOneCalendar.Model
{
    public class Race
    {
        public Race()
        {
            RaceDetails = new List<RaceDetail>();
        }
        
        public string RaceName { get; set; }
        public string Circuit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long Season { get; set; }
        public int Round { get; set; }
        public string PageId { get; set; }
        public IList<RaceDetail> RaceDetails { get; private set; }

        public void AddRaceDetails(IEnumerable<RaceDetail> details)
        {
            foreach (var detail in details)
            {
                RaceDetails.Add(detail);
            }
        }
    }
}