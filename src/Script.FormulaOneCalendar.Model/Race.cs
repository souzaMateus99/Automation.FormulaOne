using System;
using System.Xml.Serialization;

namespace Script.FormulaOneCalendar.Model
{
    [XmlRoot(ElementName="Race")]
    public class Race
    {
        [XmlElement(ElementName="RaceName")]
        public string RaceName { get; set; }

        [XmlElement(ElementName="Circuit")]
        public Circuit Circuit { get; set; }

        [XmlElement(ElementName="Date")]
        public DateTime Date { get; set; }

        [XmlElement(ElementName="Time")]
        public DateTime Time { get; set; }

        [XmlAttribute(AttributeName="season")]
        public int Season { get; set; }

        [XmlAttribute(AttributeName="round")]
        public int Round { get; set; }

        [XmlAttribute(AttributeName="url")]
        public string Url { get; set; }
    }
}