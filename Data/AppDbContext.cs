using Microsoft.EntityFrameworkCore;
using theUpSkilzAPI.Models;

namespace theUpSkilzAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
