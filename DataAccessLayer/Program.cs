using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DataAccessLayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                var users = db.Readers.ToList();
                foreach (var u in users)
                    Console.WriteLine($"{u.Id}: {u.Name}");

                var books = db.Books.ToList();
                foreach (var b in books)
                    Console.WriteLine($"{b.Id}: {b.Title}");
            }
        }
    }
}
