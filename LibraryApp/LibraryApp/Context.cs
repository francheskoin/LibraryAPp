using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Entity;

namespace LibraryApp
{
    class Context : DbContext
        
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"host=localhost;port=5432;database=Library;username=postgres;password=12345");
        }

        public DbSet<Library> Libraries { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Employer> Employees { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Fine> Fines { get; set; }

        public DbSet<SerialBooks> SerialsBooks { get; set; }

        public DbSet<SerialBooks> JobTitles { get; set; }

        public DbSet<Author> Authors { get; set; }

    }
}
