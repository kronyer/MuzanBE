using Microsoft.EntityFrameworkCore;
using Muzan.Models;

namespace Muzan.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
    }
}
