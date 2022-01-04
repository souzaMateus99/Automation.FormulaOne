using System.Collections.Generic;
using System.Xml.Serialization;

namespace Script.FormulaOneCalendar.Model
{
    [XmlRoot(ElementName="RaceTable")]
    public class RaceTable
    {

        [XmlElement(ElementName="Race")]
        public Race[] Races { get; set; }

        [XmlAttribute(AttributeName="season")]
        public int Season { get; set; }
    }
}