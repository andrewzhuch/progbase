using static System.Console;
using System;
using System.IO;
using System.Collections.Generic;

namespace lab5_2
{
    class Program
    {
        static void Main(string[] args)
        {   
            WriteLine("Command:");
            string command = ReadLine();
            if(command.StartsWith("load "))
            {
                Root root = GetCoursesFromFile(command);
                while(true)
                {
                    WriteLine("Command:");
                    string command1 = ReadLine();
                    if(command1.StartsWith("save "))
                    {
                        ProcessCommandSave(command1, root);
                    }
                    else if(command1 == "exit")
                    {
                        break;
                    }
                    else if(command1 == "prefixes")
                    {
                        ProcessPrefixes(root);
                    }
                    else if(command1 == "instructors")
                    {
                        ProcessInstructors(root);
                    }
                    else if(command1.StartsWith("titles "))
                    {
                        ProcessTitles(root, command1);
                    }
                    else if(command1.StartsWith("print "))
                    {
                        ProcessCommandPrint(root, command1);
                    }
                    else if(command1.StartsWith("export "))
                    {
                        ProcessCommandExport(command1, root);
                    }
                    else if(command1.StartsWith("image "))
                    {
                        ProcessCommandImage(root, command1);
                    }
                    else
                    {
                        WriteLine("Invalid command");
                    }
                }
            }
            else
            {
                WriteLine("Invalid command");
            }
        }
        static Root GetCoursesFromFile(string command)
        {
            string[] values = command.Split(' ');
            if(values.Length != 2)
            {
                throw new Exception("Wrong name of file");
            }
            try
            {
                StreamReader reader = new StreamReader(values[1]);
            }
            catch
            {
                throw new Exception("Wrong name of file");
            }
            Root root = XMLProcessor.DeserializeCourses(values[1]);
            return root;
        }
        static void ProcessTitles(Root root, string command)
        {
            string[] values = command.Split(' ');
            if(values.Length != 2)
            {
                throw new Exception("Wrong name of file");
            }
            List<string> titles = new List<string>();
            foreach(Course course in root.courses)
            {
                if(course.place.bilding == null)
                {
                    continue;
                }
                else if(course.place.bilding == values[1])
                {
                    titles.Add(course.title);
                }
            }
            foreach(string title in titles)
            {
                WriteLine(title);
            }
        }
        static void ProcessCommandSave(string command, Root root)
        {
            string[] values = command.Split(' ');
            if(values.Length != 2)
            {
                throw new Exception("Wrong name of file");
            }
            XMLProcessor.SerializeCourses(root, values[1]);
        }
        static void ProcessPrefixes(Root root)
        {
            HashSet<string> prefixes = new HashSet<string>();
            foreach(Course course in root.courses)
            {
                prefixes.Add(course.prefix);
            }
            foreach(string prefix in prefixes)
            {
                Console.WriteLine(prefix);
            }
        }
        static void ProcessInstructors(Root root)
        {
            HashSet<string> instructors = new HashSet<string>();
            foreach(Course course in root.courses)
            {
                instructors.Add(course.instructor);
            }
            foreach(string prefix in instructors)
            {
                Console.WriteLine(prefix);
            }
        }
        static void ProcessCommandPrint(Root root, string command)
        {
            string[] values = command.Split(' ');
            if(values.Length != 2)
            {
                throw new Exception("Wrong command");
            }
            int page;
            bool check = int.TryParse(values[1], out page);
            if(check == false)
            {
                throw new Exception("Wrong page number");
            }
            if(page > Math.Ceiling((double)root.courses.Count / 10.0))
            {
                throw new Exception("Wrong page number");
            }
            string[] array = DataProcessor.GetPages(page, root);
            WriteLine($"Total pages: {Math.Ceiling((double)root.courses.Count / 10.0)}");
            for(int i = 0; i < 10; i++)
            {
                WriteLine(array[i]);
            }
        }
        static void ProcessCommandExport(string command, Root root)
        {
            string[] values = command.Split(' ');
            if(values.Length != 3)
            {
                throw new Exception("Wrong command");
            }
            int n;
            bool check = int.TryParse(values[1], out n);
            if(check == false)
            {
                throw new Exception("Wrong page number");
            }
            Root sortedroot = DataProcessor.GetSortedByEnroll(root);
            Course[] arrayForExport = new Course[n];
            for(int i = 0; i < n; i++)
            {
                arrayForExport[i] = sortedroot.courses[i];
                XMLProcessor.SerializeSortedArray(arrayForExport, values[2]);
            }
            XMLProcessor.SerializeSortedArray(arrayForExport, values[2]);
        }
        static void ProcessCommandImage(Root root, string command)
        {
            string[] values = command.Split(' ');
            if(values.Length != 2)
            {
                throw new Exception("Wrong command");
            }
            Root sortedroot = DataProcessor.GetSortedByEnroll(root);
            Course[] arrayForImage = new Course[10];
            for(int i = 0; i < 10; i++)
            {
                arrayForImage[i] = sortedroot.courses[i];
            }
            ImageProcessor.DrawImage(arrayForImage, values[1]);
        }
    }
}
