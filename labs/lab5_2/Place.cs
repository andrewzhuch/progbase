using System.Xml.Serialization;
namespace lab5_2
{
    public class Place
    {
        [XmlElement("bldg")]
        public string bilding;
        public string room;
    }
}