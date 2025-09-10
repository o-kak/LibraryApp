using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Reader
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int ID { get; set; }
        public List<Book> BooksBorrowed { get; set; } = new List<Book>();

        public Reader(string name, string address, int id)
        {
            Name = name;
            Address = address;
            ID = id;
        }
    }
}
