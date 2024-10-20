using Domain;
using Microsoft.EntityFrameworkCore;

namespace aiProj.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }
    }
}