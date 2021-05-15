using Email.Models;
using Microsoft.EntityFrameworkCore;

namespace Email
{
    public class ApplicationContext : DbContext
    {
        private string dbName { get; set; }

        public ApplicationContext(string dbName)
        {
            this.dbName = dbName;
        }
        public DbSet<HouseMapping> HouseMappings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRoles> UserInRoles { get; set; }
        public DbSet<UserInDivision> UserInDivisions { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=dev-ws-v-07;Port=5432;Database="+ dbName + ";Username=mobnius;Password=mobnius-0");
        }
    }
}
