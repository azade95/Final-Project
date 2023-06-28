using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YatriiWorld.Models;

namespace YatriiWorld.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }

    }
}
