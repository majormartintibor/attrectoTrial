using Feed.Persistence.Configurations;
using Feed.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace Feed.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Entity.Feed> Feeds { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserFeedLike> UserFeedLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new FeedConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserFeedLikeConfiguration());
    }

    public void Seed()
    {
        SeedData.Initialize(this);
    }
}