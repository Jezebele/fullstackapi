using FullStackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FullStackAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<User> Users { get; set; }
    }
}