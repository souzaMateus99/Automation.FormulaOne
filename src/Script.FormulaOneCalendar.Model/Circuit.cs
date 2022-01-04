using System.Xml.Serialization;

namespace Script.FormulaOneCalendar.Model
{
    [XmlRoot(ElementName="Circuit")]
    public class Circuit
    {
        [XmlElement(ElementName="CircuitName")]
        public string CircuitName { get; set; }

        [XmlElement(ElementName="Location")]
        public Location Location { get; set; }

        [XmlAttribute(AttributeName="circuitId")]
        public string CircuitId { get; set; }

        [XmlAttribute(AttributeName="url")]
        public string Url { get; set; }
    }

    [XmlRoot(ElementName="Location")]
    public class Location
    { 

        [XmlElement(ElementName="Locality")]
        public string Locality { get; set; }

        [XmlElement(ElementName="Country")]
        public string Country { get; set; }

        [XmlAttribute(AttributeName="lat")]
        public double Lat { get; set; }

        [XmlAttribute(AttributeName="long")]
        public double Long { get; set; }
    }
}