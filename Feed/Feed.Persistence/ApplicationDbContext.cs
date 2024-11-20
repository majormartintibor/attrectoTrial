using Microsoft.EntityFrameworkCore;

namespace Feed.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {        
    }

    public DbSet<Entity.Feed> Feeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new FeedConfiguration());       

        //ApplyJoiningTableConfigurations(modelBuilder);
    }
}