using System.Xml.Serialization;

namespace Script.FormulaOneCalendar.Model
{
    [XmlRoot(Namespace = "http://ergast.com/mrd/1.4", ElementName="MRData")]
    public class Season
    {
        [XmlElement(ElementName="RaceTable")]
        public RaceTable RacesInfo { get; set; }

        [XmlAttribute(AttributeName="xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName="series")]
        public string Series { get; set; }

        [XmlAttribute(AttributeName="url")]
        public string Url { get; set; }

        [XmlAttribute(AttributeName="limit")]
        public int Limit { get; set; }

        [XmlAttribute(AttributeName="offset")]
        public int Offset { get; set; }

        [XmlAttribute(AttributeName="total")]
        public int Total { get; set; }
    }
}