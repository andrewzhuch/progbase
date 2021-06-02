using System.Xml.Serialization;

namespace lab5_2
{
    public class Course
    {
        public string footnote;
        public string sln; // I have no idea what is the decryption of this abbreviation:(
        public string prefix;
        public int crs; // I have no idea what is the decryption of this abbreviation:(
        public string lab;
        [XmlElement("sect")]
        public int section;
        public string title;
        public string credit;
        public string days;
        public Times times;
        public Place place;
        public string instructor;
        public int limit;
        public int enrolled;
        public string ConvertToSting()
        {
            return $"Title: {this.title}, section: {this.section}, enrolled: {this.enrolled}";
        }

    }
}