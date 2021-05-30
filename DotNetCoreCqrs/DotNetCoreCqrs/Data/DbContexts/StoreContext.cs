using DotNetCoreCqrs.Models.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.Data.DbContexts
{
    public class StoreContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Owner> Owners { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /* when we need to store enum as string in db
            modelBuilder
                .Entity<Car>()
                .Property(e => e.Model)
                .HasConversion(
                    v => v.ToString(),
                    v => (CARMODEL)Enum.Parse(typeof(CARMODEL), v));
            */

            modelBuilder.Entity<Car>().HasOne(c => c.owner).WithMany(o => o.cars).OnDelete(DeleteBehavior.Cascade);
        }


        //https://stackoverflow.com/questions/36488461/sqlite-in-asp-net-core-with-entityframeworkcore
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
