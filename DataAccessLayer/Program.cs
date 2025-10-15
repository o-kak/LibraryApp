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
                db.Readers.Add(new Reader("Alice", "aaa"));
                db.SaveChanges();

                var users = db.Readers.ToList();
                foreach (var u in users)
                    Console.WriteLine($"{u.Id}: {u.Name}");
            }
        }
    }
}
