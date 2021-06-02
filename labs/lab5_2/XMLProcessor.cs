using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
namespace lab5_2
{
    public static class XMLProcessor
    {
        public static Root DeserializeCourses(string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Root));
            StreamReader reader = new StreamReader(path);
            Root courses = (Root)ser.Deserialize(reader);
            reader.Close();
            return courses;
        }
        public static void SerializeCourses(Root root, string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Root));
            StreamWriter writer = new StreamWriter(path);
            ser.Serialize(writer, root);
            writer.Close();
        }
        public static void SerializeSortedArray(Course[] array, string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Course[]));
            StreamWriter writer = new StreamWriter(path);
            ser.Serialize(writer, array);
            writer.Close();
        }
    }
}