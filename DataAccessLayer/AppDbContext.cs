using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Book> Books { get; set; }
        public AppDbContext() : base("name=PostgresConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

    }
}
