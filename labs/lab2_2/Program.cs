using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace lab2_2
{
    class Site
    {
        public int id;
        public string address;
        public string topic;
        public int numberOfVisitors;
        public Site()
        {
            id = 0;
            address = null;
            topic = null;
            numberOfVisitors = 0;
        }
        public Site(int idNumber, string link, string topicOfSite, int visitors)
        {
            id = idNumber;
            address = link;
            topic = topicOfSite;
            numberOfVisitors = visitors;
        }
        public override string ToString()
        {
            return $"ID: {this.id}, adress: {this.address}, topic of the site: {this.topic}, avarage number of visitors in month: {this.numberOfVisitors}";
        }
        public string ConvertToCsvRaw()
        {
            return $"{this.id},{this.address},{this.topic},{this.numberOfVisitors}";
        }
        public int GetNumberOfVisitors()
        {
            return this.numberOfVisitors;
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
    class SiteRepository
    {
        private SqliteConnection _connection;
        public SiteRepository(SqliteConnection connection)
        {
            this._connection = connection;
        }
        public Site GetById(int id)
        {
            SqliteCommand command = this._connection.CreateCommand();
            command.CommandText = @"SELECT * FROM sites WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Site site = new Site();
                site.id = int.Parse(reader.GetString(0));
                site.address = reader.GetString(1);
                site.topic = reader.GetString(2);
                site.numberOfVisitors = int.Parse(reader.GetString(3));
                reader.Close();
                return site;
            }
            else
            {
                reader.Close();
                throw new Exception($"There is no site with id {id}");
            }
        }
        public int DeleteById(int id)
        {
            SqliteCommand command = this._connection.CreateCommand();
            command.CommandText = @"DELETE FROM sites WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int changes = command.ExecuteNonQuery();
            if (changes == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private long GetCount()
        {
            SqliteCommand command = this._connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM sites";
            long count = (long)command.ExecuteScalar();
            return count;
        }
        public int GetTotalPages()
        {
            const int pageSize = 10;
            return (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        }
        public long Insert(Site site)
        {
            SqliteCommand command = this._connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO sites (address, topic, numberofvisitors) 
                VALUES ($address, $topic, $numberofvisitors);

                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$address", site.address);
            command.Parameters.AddWithValue("$topic", site.topic);
            command.Parameters.AddWithValue("$numberofvisitors", site.numberOfVisitors);
            long newId = (long)command.ExecuteScalar();
            return newId;
        }
        public ListSite GetExpot(string valueX)
        {
            SqliteCommand command = this._connection.CreateCommand();
            command.CommandText = @"SELECT * FROM sites WHERE topic NOT LIKE '%' || $valueX || '%'";
            command.Parameters.AddWithValue("$valueX", valueX);
            ListSite sites = new ListSite();
            SqliteDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                Site site = new Site();
                site.id = int.Parse(reader.GetString(0));
                site.address = reader.GetString(1);
                site.topic = reader.GetString(2);
                site.numberOfVisitors = int.Parse(reader.GetString(3));
                sites.Add(site);
            }
            reader.Close();
            return sites;
        }
        public ListSite GetPage(int pageNumber)
        {
            long pageSize = 10;
            if(pageNumber > this.GetTotalPages())
            {
                throw new Exception("Wrong page number");
            }
            SqliteCommand command = this._connection.CreateCommand();
            command.CommandText = @"SELECT * FROM sites LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumber", pageNumber);
            SqliteDataReader reader = command.ExecuteReader();
            ListSite sites = new ListSite();
            while(reader.Read())
            {
                Site site = new Site();
                site.id = int.Parse(reader.GetString(0));
                site.address = reader.GetString(1);
                site.topic = reader.GetString(2);
                site.numberOfVisitors = int.Parse(reader.GetString(3));
                sites.Add(site);
            }
            reader.Close();
            return sites;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
            string command = "";
            do
            {
                Console.WriteLine("Please, input your command:");
                command = Console.ReadLine();
                if(command.StartsWith("getById "))
                {
                    ProccessingGetById(command);
                }
                if(command.StartsWith("deleteById "))
                {
                    ProccessingDeleteById(command);
                }
                if(command.StartsWith("insert "))
                {
                    ProccessingInsert(command);
                }
                if(command == "getTotalPages")
                {
                    ProccessingGetTotalPages();
                }
                if(command.StartsWith("getPage"))
                {
                    ProccessingGetPage(command);
                }
                if(command.StartsWith("export"))
                {
                    ProccessingExport(command);
                }
            }
            while(command != "exit");
        }
        static Site ConvertInputToSite(string raw)
        {
            raw = raw.Substring(7);
            string[] parameters = raw.Split(',');
            CheckDataForSite(parameters);
            Site site = new Site();
            site.address = parameters[0];
            site.topic = parameters[1];
            site.numberOfVisitors = int.Parse(parameters[2]);
            return site;
        }
        static void CheckDataForSite(string[] data)
        {
            if(data.Length != 3)
            {
                throw new Exception("Wrong number of parameters");
            }
            int a;
            bool isCorrect = int.TryParse(data[2], out a);
            if(isCorrect == false)
            {
                throw new Exception("Wrong numberOfVisitors parameter, input an integer");
            }
        }
        static void WriteAllSites(string file, ListSite sites)
        {
            StreamWriter sw = new StreamWriter(file);
            for(int i = 0; i < sites.GetCount(); i++)
            {
                sw.WriteLine(sites.GetAt(i).ConvertToCsvRaw());
            }
            sw.Close();
        }
        static void ProccessingGetById(string command)
        {
            string dataBaseFile = "./sitesdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SiteRepository repository = new SiteRepository(connection);
            string[] array = command.Split(' ');
            int a;
            bool isCorrect = int.TryParse(array[1], out a);
            if(array.Length != 2)
            {
                throw new Exception("Wrong input");
            }
            if(isCorrect == false)
            {
                throw new Exception("Wrong input");
            }
            Console.WriteLine(repository.GetById(a));
            connection.Close();
        }
        static void ProccessingDeleteById(string command)
        {
            string dataBaseFile = "./sitesdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SiteRepository repository = new SiteRepository(connection);
            string[] array = command.Split(' ');
            int a;
            bool isCorrect = int.TryParse(array[1], out a);
            if(array.Length != 2)
            {
                throw new Exception("Wrong input");
            }
            if(isCorrect == false)
            {
                throw new Exception("Wrong input");
            }
            Console.WriteLine(repository.DeleteById(a));
            connection.Close();
        }
        static void ProccessingInsert(string command)
        {
            string dataBaseFile = "./sitesdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SiteRepository repository = new SiteRepository(connection);
            Site site = ConvertInputToSite(command);
            Console.WriteLine(repository.Insert(site));
            connection.Close();
        }
        static void ProccessingGetTotalPages()
        {
            string dataBaseFile = "./sitesdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SiteRepository repository = new SiteRepository(connection); 
            Console.WriteLine(repository.GetTotalPages());
            connection.Close();
        }
        static void ProccessingGetPage(string command)
        {
            string dataBaseFile = "./sitesdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SiteRepository repository = new SiteRepository(connection);
            string[] array = command.Split(' ');
            int a;
            bool isCorrect = int.TryParse(array[1], out a);
            if(array.Length != 2)
            {
                throw new Exception("Wrong input");
            }
            if(isCorrect == false)
            {
                throw new Exception("Wrong input");
            }
            ListSite sites = repository.GetPage(a);
            for(int i = 0; i < sites.GetCount(); i++)
            {
                Console.WriteLine(sites.GetAt(i));
            }
            connection.Close();
        }
        static void ProccessingExport(string command)
        {
            string outputFile = "./export.csv";
            string dataBaseFile = "./sitesdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SiteRepository repository = new SiteRepository(connection);
            string[] array = command.Split(' ');
            if(array.Length != 2)
            {
                throw new Exception("Wrong input");
            }
            Console.WriteLine("The name of output file is export.csv");
            WriteAllSites(outputFile, repository.GetExpot(array[1]));
            connection.Close();
        }
    }
}
