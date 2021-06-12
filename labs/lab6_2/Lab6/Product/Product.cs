using System;

namespace Product
{
    public class Product
    {
        public long id;
        public string name;
        public string info;
        public int price;
        public bool onStorage;
        public DateTime createdAt;
        public override string ToString()
        {
            return $"id - {this.id}, name - {this.name}, created at - {this.createdAt};";
        }
    }
}
