using System.Collections.Generic;
using System.Xml.Serialization;

namespace lab5_2
{
    [XmlRoot("root")]
    public class Root
    {
        [XmlElement("course")]
        public List<Course> courses;
    }
}