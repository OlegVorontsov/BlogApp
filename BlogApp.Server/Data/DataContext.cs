using Microsoft.EntityFrameworkCore;

namespace BlogApp.Server.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<UserSub> UserSubs { get; set; }
        public DbSet<NewsLike> NewsLikes { get; set; }
        public DataContext(DbContextOptions<DataContext> contextOptions) : base(contextOptions)
        {
            Database.EnsureCreated();
        }
    }
}
