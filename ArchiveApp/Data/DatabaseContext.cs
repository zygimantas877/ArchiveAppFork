using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiveApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchiveApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArchiveAppDB;Integrated Security=True;");
        }

        ~DatabaseContext()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
