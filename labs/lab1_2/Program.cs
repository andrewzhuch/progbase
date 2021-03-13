using System;
using System.IO;

namespace lab1_2
{
    class Site
    {
        private int _id;
        private string _address;
        private string _topic;
        private int _numberOfVisitors;
        public Site()
        {
            _id = 0;
            _address = null;
            _topic = null;
            _numberOfVisitors = 0;
        }
        public Site(int idNumber, string link, string topicOfSite, int visitors)
        {
            _id = idNumber;
            _address = link;
            _topic = topicOfSite;
            _numberOfVisitors = visitors;
        }
        public override string ToString()
        {
            return $"ID: {this._id}, adress: {this._address}, topic of the site: {this._topic}, avarage number of visitors in month: {this._numberOfVisitors}";
        }
        public string ConvertToCsvRaw()
        {
            return $"{this._id},{this._address},{this._topic},{this._numberOfVisitors}";
        }
        public int GetNumberOfVisitors()
        {
            return this._numberOfVisitors;
        }
    }
    class ListSite
    {
        private Site[] _items;
        private int _size;
        public ListSite()
        {
            _items = new Site[16];
            _size = 0;
        }
        private void EnsureCapasity(int newSize)
        {
           Array.Resize(ref this._items, newSize);
        }
        private void MoveLementsLeft(int index)
        {
            for(int i = index; i < this._size - 1; i ++)
            {
                this._items[i] = this._items[i + 1];
            }
            this._size --;
        }
        private void MoveElementsRight(int index)
        {
            if(this._size == this._items.Length)
            {
                EnsureCapasity(this._items.Length * 2);
            } 
            for(int i = this._size; i > index; i--)
            {
                this._items[i] = this._items[i - 1];
            }
            this._size ++;
        }
        public void Add(Site newSite)
        {
            if(this._size == this._items.Length)
            {
                EnsureCapasity(this._items.Length * 2);
            }
            this._items[this._size] = newSite;
            this._size ++;
        }
        public void Insert(int index, Site newSite)
        {
            if(index > this._size || index < 0)
            {
                throw new Exception("There is no such index in the list");
            }
            else if(index == this._size)
            {
                this.Add(newSite);
            }
            else
            {
                this.MoveElementsRight(index);
                this._items[index] = newSite;
            }
        }
        public bool Remove(Site site)
        {
            for(int i = 0; i < this._size; i++)
            {
                if(site == this._items[i])
                {
                    MoveLementsLeft(i);
                    return true;
                }
            }
            return false;
        }
        public void RemoveAt(int index)
        {
            if(index < 0 || index > this._size - 1)
            {
                throw new Exception("There is no such index in the list");
            }
            MoveLementsLeft(index);
        }
        public void Clear()
        {
            for(int i = 0; i < this._size; i++)
            {
                this._items[i] = null;
            }
            this._size = 0;
        }
        public int GetCount()
        {
            return this._size;
        }
        public int GetCapacity()
        {
            return this._items.Length;
        }
        public Site GetAt(int index)
        {
            if(index < 0 || index > this._size - 1)
            {
                throw new Exception("There is no such index in the list");
            }
            return this._items[index];
        }
        public void SetAt(int index, Site site)
        {
            if(index < 0 || index > this._size - 1)
            {
                throw new Exception("There is no such index in the list");
            }
            this._items[index] = site;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string outputFile;
            int numberOfRaws;
            CheckCommandlineArgs(args, out outputFile, out numberOfRaws);
            CreateCsv(outputFile, numberOfRaws);
            ListSite sites = ReadAllSites("./sites.csv");
            ListSite sites1 = ReadAllSites("./sites1");
            Console.WriteLine("There are {0} raws in first file", sites.GetCount());
            Console.WriteLine("Here are fisrt 10 raws from first file:");
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(sites.GetAt(i).ConvertToCsvRaw());
            }
            Console.WriteLine("There are {0} raws in second file", sites1.GetCount());
            Console.WriteLine("Here are fisrt 10 raws from second file:");
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(sites1.GetAt(i).ConvertToCsvRaw());
            }
            ListSite newSites = UnionOfTwoLists(sites, sites1);
            int avarage = GetAvarage(newSites);
            RemoveElements(newSites, avarage);
            WriteAllEntities("./output", newSites);
        }
        static bool CheckCommandlineArgs(string[] args, out string nameOfFile, out int numberOfRaws)
        {
            if(args.Length < 2)
            {
                throw new Exception("Not all arguments were given.");
            }
            if(args.Length > 2)
            {
                throw new Exception("Too many arguments were givem.");
            }
            int numberOfStrings;
            bool isNumberOK = int.TryParse(args[1], out numberOfStrings);
            if(isNumberOK == false)
            {
                throw new Exception("Wrong second argument. It should be an ineger");
            }
            else if(numberOfStrings < 0)
            {
                throw new Exception("Wrong number of raws.");
            }
            else
            {
                nameOfFile = args[0];
                numberOfRaws = numberOfStrings;
                return true;
            }
        }
        static void CreateCsv(string outputFile, int numberOfRaws)
        {
            StreamWriter writer = new StreamWriter(outputFile);
            writer.WriteLine("id,address,topic,number of visitors");
            Random random = new Random();
            for(int i = 0; i < numberOfRaws; i++)
            {
                Site site = new Site(i + 1, GetRandomString(), GetRandomString(), random.Next(1000, 10000000));
                string csvRaw = site.ConvertToCsvRaw();
                writer.WriteLine(csvRaw);  
            }
            writer.Close();
        }
        static string GetRandomString()
        {
            Random random = new Random();
            int length = random.Next(2, 15);
            char[] chars = new char[length];
            for(int i = 0; i < length; i++)
            {
                int randomChar = random.Next((int)'a', (int)'z' + 1);
                chars[i] = (char)randomChar;
            }
            return new string(chars);
        }
        static ListSite ReadAllSites(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string s = null;
            ListSite sites = new ListSite();
            int counter = 0; // this counter created in order to skip first raw in CSV, cause first raw contains names of parameters
            while(true)
            {   
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                if(counter != 0)
                {
                    string[] parameters = s.Split(',');
                    CheckCsvRaw(parameters);
                    Site site = new Site(int.Parse(parameters[0]), parameters[1], parameters[2], int.Parse(parameters[3]));
                    sites.Add(site);
                }
                counter++;
            }
            sr.Close();
            return sites;
        }
        static ListSite UnionOfTwoLists(ListSite sites, ListSite sites1)
        {
            ListSite newSites = new  ListSite();
            for(int i = 0; i < sites.GetCount(); i++)
            {
                newSites.Add(sites.GetAt(i));
            }
            for(int i = 0; i < sites1.GetCount(); i++)
            {
                newSites.Add(sites1.GetAt(i));
            }
            return newSites;
        }
        static int GetAvarage(ListSite sites)
        {
            int summ = 0;
            for(int i = 0; i < sites.GetCount(); i++)
            {
                summ = summ + sites.GetAt(i).GetNumberOfVisitors();
            }
            return summ / sites.GetCount();
        }
        static void RemoveElements(ListSite sites, int avarage)
        {
            for(int i = 0; i < sites.GetCount(); i++)
            {
                if(sites.GetAt(i).GetNumberOfVisitors() < avarage)
                {
                    sites.RemoveAt(i);
                }
            }
        }
        static void WriteAllEntities(string file, ListSite sites)
        {
            StreamWriter sw = new StreamWriter(file);
            for(int i = 0; i < sites.GetCount(); i++)
            {
                sw.WriteLine(sites.GetAt(i).ConvertToCsvRaw());
            }
            sw.Close();
        }
        static void CheckCsvRaw(string[] raw)
        {
            int a;
            bool isCorrect = int.TryParse(raw[0], out a);
            int b;
            bool isCorrect1 = int.TryParse(raw[3], out b);
            if(isCorrect == false || isCorrect1 == false)
            {
                throw new Exception("Wrong data in files");
            }
            if(raw.Length != 4)
            {
                throw new Exception("Wrong data in files");
            }
        }
    }
}
